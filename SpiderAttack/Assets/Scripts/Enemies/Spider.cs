using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.enums;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spider: MonoBehaviour, IListener
{

#if UNITY_4_5
    [Header("Graphics")]
#endif
    public SkeletonAnimation skeletonAnimation;
#if UNITY_4_5
    [Header("Animation")]
#endif
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string idle = "Idle";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string walk = "Walk";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string attack_melee = "attack_melee";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string attack_range = "attack_range";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string hurt = "Hurt";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string death = "Death";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string webAnimation = "WebAnimation";

    public BattleController battleController;
    public Transform spiderTarget;
    public int rangeDamage = 50;

    public GameObject spiderWebBullet;
    protected bool _rightDirect;
    protected string _currentAnim;
    public float speed = 2;
    protected float _speed;
    public float speedWalking = 1;
    public float speedAttackMelee = 1;
    public float speedAttackRange = 1;
    public float speedDeath = 0.5f;
    public float speedHurt = 1f;
    protected List<Coroutine> coroutines;
    public int hp;
    public Slider hpSlider;
    public int meleeDamage = 100;
    public float meleeRange = 3;
    protected bool _forceAttack; //when need to range attack immediately
    public float randomAttackValue; //1- range, 0 - melee
    public string bodyPattern;
    public Color skinColor;
    protected bool isReach;
    protected bool isDeath;
    protected SpiderAttackState currentState;
    protected BulletType currentBullet;
    private Coroutine _coWalkAttack;
    private Coroutine _coWalkAway;
    private Transform _spiderAim;


    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.GateDestroy, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SuperBomb, this);
        EventManager.Instance.AddListener(EVENT_TYPE.MainHouseDestroy, this);
        coroutines = new List<Coroutine>();

        _speed = speed;
        if (hpSlider != null)
        {
            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }

        skeletonAnimation.skeleton.FindSlot(bodyPattern).SetColor(skinColor);
        //bool random = (Random.value < 0.5f);
        SpiderWalkToTarget();

        skeletonAnimation.state.Event += delegate (TrackEntry entry, Spine.Event e)
        {
            if (e.Data.Name == "Start_animations_folder/start_attack_range")
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.SpiderStartRangeAttack, this);
            }

            if (e.Data.Name == "Start_animations_folder/start_animation_web_1")
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.SpiderSpitRangeAttack, this);
            }

            if (e.Data.Name == "melee_attack_hit")
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.SpiderMeleeAttackGate, this,
                    meleeDamage);
            }

            if (e.Data.Name == "start_animation/start_range_attack")
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.SpiderStartRangeAttack, this);
                isReach = true;
            }

            else if (e.Data.Name == "start_animation/start_web_animation")
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.SpiderSpitRangeAttack, this);
                isReach = true;
            }

        };
    }

    protected TrackEntry SetAnimation(string animationName, bool loop, float speedAnimation = 1, int trackIndex = 0)
    {
        var anim = skeletonAnimation.state.SetAnimation(trackIndex, animationName, loop);
        anim.TimeScale = speedAnimation;
        if (animationName != hurt)
        {
            _currentAnim = animationName;
        }

        return anim;
    }

    protected abstract IEnumerator WalkAttack(SpiderAttackType spiderAttackState, Transform target, Vector3 dir);

    protected abstract IEnumerator WalkAway(float time, Vector3 vector3);


    public void SpiderWalkToTarget()
    {
        currentState = SpiderAttackState.InWalkingToTarget; 
        var target = FindTarget();

        if (target == null)
        {
            SetAnimation(idle, true);
            return;
        }
        var targetSide = CheckTargetSide();

        var randomBool = Random.value < randomAttackValue;
        var attackState = randomBool ? SpiderAttackType.RangeAttack : SpiderAttackType.MeleeAttack;
        if (_forceAttack)
        {
            attackState = SpiderAttackType.RangeAttack;
        }

        if (targetSide == TargetSide.TargetOnLeft)
        {
            StopAnimCoroutines();
            //var co = StartCoroutine(WalkAttack(attackState, target, Vector3.left));
            _coWalkAttack = StartCoroutine(WalkAttack(attackState, target, Vector3.left));
            coroutines.Add(_coWalkAttack);
        }
        else
        {
            StopAnimCoroutines();
            _coWalkAttack = StartCoroutine(WalkAttack(attackState, target, Vector3.right));
            coroutines.Add(_coWalkAttack);
        }

    }

    public void SpiderWalkFromTarget()
    {
        currentState = SpiderAttackState.InWalkingFromTarget;
        var target = FindTarget();
        var targetSide = CheckTargetSide();
        if (targetSide == TargetSide.TargetOnLeft)
        {


            StopAnimCoroutines();
            _coWalkAway = StartCoroutine(WalkAway(2, Vector3.right));
            coroutines.Add(_coWalkAway);
        }
        else
        {
            StopAnimCoroutines();
            _coWalkAway = StartCoroutine(WalkAway(2, Vector3.left));
            coroutines.Add(_coWalkAway);
        }
    }


    protected TargetSide CheckTargetSide()
    {
        if (gameObject.transform.position.x > battleController.spiderTarget.position.x)
        {
            return TargetSide.TargetOnLeft;
        }

        return TargetSide.TargetOnRight;

    }

    protected void AnimRange_Complete2(TrackEntry trackEntry)
    {
        if (_forceAttack)
        {
            SpiderWalkToTarget();
            return;
        }

        var val = Random.value;
        var myBool = (val < .5);
        if (myBool)
        {
            SpiderWalkFromTarget();
        }
    }

    protected void AnimMelee_Complete(TrackEntry trackEntry)
    {
        if (_forceAttack)
        {
            SpiderWalkToTarget();
            return;
        }

        var val = Random.value;
        var myBool = (val < .5);
        if (myBool)
        {
            SpiderWalkFromTarget();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        StopAnimCoroutines();
        if (collision.gameObject.tag == "BulletStone")
        {
            _speed = 0;

            currentBullet = collision.gameObject.GetComponent<Bullet>()?.bulletType ?? BulletType.Arrow;


            if (hpSlider != null)
            {
                isDeath = ChangeHp(collision.gameObject.GetComponent<Bullet>().Damage);
            }
            if (!isDeath)
            {
                var trackEntryHurt = SetAnimation(hurt, false, speedHurt);
                trackEntryHurt.Complete += AnimHurt_Complete;
            }

            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderHurt, this, collision.gameObject.GetComponent<Bullet>().Damage);
        }
    }

   

    protected void AnimHurt_Complete(TrackEntry trackEntry)
    {

        if (currentBullet == BulletType.Arrow)
        {

            if (currentState == SpiderAttackState.InWalkingToTarget)
            {
                SpiderWalkToTarget();
            }
            else if (currentState == SpiderAttackState.InWalkingFromTarget)
            {
                SpiderWalkFromTarget();
            }
        }
        else if (currentBullet == BulletType.Ball)
        {
            SpiderWalkFromTarget();
        }
    }

    protected bool ChangeHp(int damage)
    {
        hp -= damage;
        hpSlider.value -= damage;

        if (hp <= 0)
        {
            StopAnimCoroutines();

            Death();
            return true;
        }
        return false;
    }

    protected virtual void Death()
    {
        gameObject.layer = Layer.SpiderDead;
        var trackEntryDeath = SetAnimation(death, false, speedDeath);
        _speed = 0;
        trackEntryDeath.Complete += AnimDeath_Complete;
    }

    protected void AnimDeath_Complete(TrackEntry trackEntry)
    {
        StopAnimCoroutines();
        EventManager.Instance.PostNotification(EVENT_TYPE.SpiderDie, this, gameObject);
        DestroyImmediate(spiderWebBullet);
        DestroyImmediate(this.gameObject);
    }


    protected void StopAnimCoroutines()
    {
        if (coroutines.Count == 0)
        {
            return;
        }

        for (int i = 0; i < coroutines.Count; i++)
        {
            if (coroutines[i] != null)
            {
                StopCoroutine(coroutines[i]);
                coroutines[0] = null;
                coroutines.Remove(coroutines[i]);
            }
        }
    }

    protected void CheckFlip(Vector3 vector3)
    {
        if (vector3 == Vector3.left && _rightDirect) Flip();
        if (vector3 == Vector3.right && !_rightDirect) Flip();

        if (_rightDirect)
        {
            Vector3 scale = hpSlider.transform.localScale;
            scale.x *= -1;
            hpSlider.transform.localScale = scale;

        }
        else
        {
            Vector3 scale = hpSlider.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            hpSlider.transform.localScale = scale;
        }
    }
    protected void Flip()
    {
        _rightDirect = !_rightDirect;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GateDestroy:
                _forceAttack = true;
                break;

            case EVENT_TYPE.SuperBomb:
            {
                if (Param == null) return;
                var superBombParam = (SuperBombParam) Param;
                //Debug.Log(Vector2.Distance(transform.position, superBombParam.position));

                if (Vector2.Distance(transform.position, superBombParam.position) > 6) //superbomb range
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.ArrowMissedTarget, this);
                        return;
                }
                EventManager.Instance.PostNotification(EVENT_TYPE.BallHitTarget, this);

                    _speed = 0;
                currentBullet = BulletType.Ball;
                if (hpSlider != null)
                {
                    isDeath = ChangeHp(superBombParam.Damage);
                }

                if (!isDeath)
                {
                    var trackEntryHurt = SetAnimation(hurt, false, speedHurt);
                    trackEntryHurt.Complete += AnimHurt_Complete;
                }

                EventManager.Instance.PostNotification(EVENT_TYPE.SpiderHurt, this, superBombParam.Damage);
                break;

            }

            case EVENT_TYPE.MainHouseDestroy:
                    SetAnimation(idle, true);
                break;
        }
    }

    public enum TargetSide
    {
        TargetOnRight,
        TargetOnLeft,
    }

    void Awake()
    {
        battleController = FindObjectOfType<BattleController>();
    }

    public Transform FindTarget()
    {
        if (gameObject == null)
        {
            return null;
        }

        _spiderAim = GetClosestEnemy(battleController.allTargets);


        battleController.spiderTarget = _spiderAim;
        spiderTarget = _spiderAim;

        return _spiderAim;
    }

    private Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        foreach (Transform t in enemies)
        {
            if (t == null) continue;
            float dist = Vector2.Distance(t.position, transform.position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }


}

public enum SpiderAttackType
{
    MeleeAttack,
    RangeAttack,
}


public enum SpiderAttackState
{
    InAttack,
    InWalkingToTarget,
    InWalkingFromTarget,
}
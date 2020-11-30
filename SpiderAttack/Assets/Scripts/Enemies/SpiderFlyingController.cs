using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpiderFlyingController : MonoBehaviour, IListener
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

    //public Transform targetPos;
    public GameObject spiderWebBullet;
    public BattleController battleController;
    private bool _rightDirect;
    private string _currentAnim;
    public float speed = 2;
    private float _speed;
    public float speedWalking = 1;
    public float speedAttackMelee = 1;
    public float speedAttackRange = 1;
    public float speedDeath = 0.5f;
    private Slot _webSlot;
    public List<Coroutine> _coroutines;
    public int hp;
    public Slider hpSlider;
    public int meleeDamage = 100;
    public int rangeDamage = 50;

    public float meleeRange = 3;

    private bool _forceAttack; //when need to range attack immediately
    public float randomAttackValue; //1- range, 0 - melee
    public Transform spiderTarget;
    public string bodyPattern;

    public Color skinColor;


    enum SpiderAttackState
    {
        MeleeAttack,
        RangeAttack,
    }

    void Awake()
    {
        battleController = (BattleController)FindObjectOfType(typeof(BattleController));
    }


    void Start()
    {

        EventManager.Instance.AddListener(EVENT_TYPE.GateDestroy, this);
        _coroutines = new List<Coroutine>();

        _speed = speed;
        if (hpSlider != null)
        {
            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }

        //skeletonAnimation.skeleton.FindSlot("body_pattern1").SetColor(skinColor);
        //skeletonAnimation.skeleton.FindSlot("body_pattern_3").SetColor(skinColor);
        skeletonAnimation.skeleton.FindSlot(bodyPattern).SetColor(skinColor);
        //bool random = (Random.value < 0.5f);
        SpiderWalkToTarget();
    }

    private TrackEntry SetAnimation(string animationName, bool loop, float speedAnimation = 1, int trackIndex = 0)
    {
        var anim = skeletonAnimation.state.SetAnimation(trackIndex, animationName, loop);
        anim.TimeScale = speedAnimation;
        _currentAnim = animationName;

        return anim;
    }

    private IEnumerator WalkAttack(SpiderAttackState spiderAttackState, Transform target, Vector3 dir)
    {

        float x = Random.Range(12, 15);
        float y = Random.Range(0, 3.5f);
        Vector2 target2 = new Vector2(x,y);

        bool isReach = false;

        _speed = speed;
        CheckFlip(dir);

        SetAnimation(walk, true, speedWalking);
        var rangeRange = Random.Range(3, 7);





        while (!isReach)
        {
            float step2 = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target2, step2);
            if (Vector2.Distance(transform.position, target2) < 0.1)
            {
                isReach = true;
            }




            if (spiderAttackState == SpiderAttackState.MeleeAttack && Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y)) < meleeRange)
            {
                var animMelee = SetAnimation(attack_melee, true, speedAttackMelee);
                animMelee.Complete += AnimMelee_Complete;

                skeletonAnimation.state.Event += delegate (TrackEntry entry, Spine.Event e)
                {
                    if (e.Data.Name == "melee_attack_hit")
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.SpiderMeleeAttackGate, this,
                            meleeDamage);
                    }

                };
                break;
            }
            if (spiderAttackState == SpiderAttackState.RangeAttack && isReach)
            {
                var animRange = SetAnimation(attack_range, true, speedAttackRange);

                animRange.Complete += AnimRange_Complete2;
                skeletonAnimation.state.Event += delegate (TrackEntry entry, Spine.Event e)
                {
                    if (e.Data.Name == "start_animation/start_range_attack")
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.SpiderStartRangeAttack, this);
                    }

                    if (e.Data.Name == "start_animation/start_web_animation")
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.SpiderSpitRangeAttack, this);
                    }
                    isReach = true;
                };
                break;
            }

           

            yield return null;
        }

    }

    private IEnumerator WalkAway(float time, Vector3 vector3)
    {
        _speed = speed;
        CheckFlip(Vector3.right);

        float elapsedTime = 0;
        var animWalk = SetAnimation(walk, true, speedWalking);


        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.Translate(vector3 * _speed * Time.deltaTime);
            yield return null;
        }


        bool random = (Random.value < 0.5f);
        SpiderAttackState spA = random ? SpiderAttackState.RangeAttack : SpiderAttackState.MeleeAttack;////////
        SpiderWalkToTarget();

    }

    public void SpiderWalkToTarget()
    {
        var target = FindTarget();

        if (target == null)
        {
            SetAnimation(idle, true);
            return;
        }
        var targetSide = CheckTargetSide();

        var randomBool = Random.value < randomAttackValue;
        var attackState = randomBool ? SpiderAttackState.RangeAttack : SpiderAttackState.MeleeAttack;
        if (_forceAttack)
        {
            attackState = SpiderAttackState.RangeAttack;
        }

        if (targetSide == TargetSide.TargetOnLeft)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkAttack(attackState, target, Vector3.left));
            _coroutines.Add(co);
        }
        else
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkAttack(attackState, target, Vector3.right));
            _coroutines.Add(co);
        }

    }

    public void SpiderWalkFromTarget()
    {
        var target = FindTarget();
        var targetSide = CheckTargetSide();
        if (targetSide == TargetSide.TargetOnLeft)
        {
            StopAnimCoroutines();
            // var co = StartCoroutine(WalkAway(2, Vector3.right));
            var co = StartCoroutine(WalkAway(2, Vector3.right));
            _coroutines.Add(co);
        }
        else
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkAway(2, Vector3.left));
            _coroutines.Add(co);
        }
    }

    public Transform FindTarget()
    {
        if (gameObject == null)
        {
            return null;
        }
        var aim = battleController.allTargets?.OrderBy(x => Vector2.Distance(transform.position, x.position)).FirstOrDefault();
        battleController.spiderTarget = aim;
        spiderTarget = aim;

        return aim;
    }

    private TargetSide CheckTargetSide()
    {
        if (gameObject.transform.position.x > battleController.spiderTarget.position.x)
        {
            return TargetSide.TargetOnLeft;
        }

        return TargetSide.TargetOnRight;

    }

    private void AnimRange_Complete2(TrackEntry trackEntry)
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

    private void AnimMelee_Complete(TrackEntry trackEntry)
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
        if (collision.gameObject.tag == "BulletStone")
        {
            _speed = 0;

            var animHurt = skeletonAnimation.state.SetAnimation(0, hurt, false);
            animHurt.Complete += AnimHurt_Complete;
            animHurt.TimeScale = 1.5f;

            Destroy(collision.gameObject);

            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderHurt, this, collision.gameObject.GetComponent<TrebuchetBullet>().Damage);

            if (hpSlider != null)
            {
                ChangeHp(collision.gameObject.GetComponent<TrebuchetBullet>().Damage);
            }

        }
    }

    private void AnimHurt_Complete(TrackEntry trackEntry)
    {
        SpiderWalkFromTarget();

    }

    private void ChangeHp(int damage)
    {
        hp -= damage;
        hpSlider.value -= damage;

        if (hp <= 0)
        {
            var animDeath = skeletonAnimation.state.SetAnimation(0, death, false);
            animDeath.TimeScale = speedDeath;
            _speed = 0;
            animDeath.Complete += AnimDeath_Complete;
        }
    }

    private void AnimDeath_Complete(TrackEntry trackEntry)
    {

        StopAnimCoroutines();
        EventManager.Instance.PostNotification(EVENT_TYPE.SpiderDie, this, gameObject);
        DestroyImmediate(spiderWebBullet);
        DestroyImmediate(this.gameObject);
    }


    private void StopAnimCoroutines()
    {
        if (!_coroutines.Any())
        {
            return;
        }
        foreach (var coroutine in _coroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    private void CheckFlip(Vector3 vector3)
    {
        if (vector3 == Vector3.left && _rightDirect) Flip();
        if (vector3 == Vector3.right && !_rightDirect) Flip();
    }
    private void Flip()
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
        }
    }

    enum TargetSide
    {
        TargetOnRight,
        TargetOnLeft,
    }
}
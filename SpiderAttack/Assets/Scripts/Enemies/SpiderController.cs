using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.enums;
using Spine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpiderController : Spider, IListener
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

    public GameObject spiderWebBullet;
    private bool _rightDirect;
    private string _currentAnim;
    public float speed = 2;
    private float _speed;
    public float speedWalking = 1;
    public float speedAttackMelee = 1;
    public float speedAttackRange = 1;
    public float speedDeath = 0.5f;
    public float speedHurt = 1f;
    private Slot _webSlot;
    public List<Coroutine> _coroutines;
    public int hp;
    public Slider hpSlider;
    public int meleeDamage = 100;
    

    public float meleeRange = 3;

    private bool _forceAttack; //when need to range attack immediately
    public float randomAttackValue; //1- range, 0 - melee
    //public Transform spiderTarget;
    public string bodyPattern;

    public Color skinColor;

    private bool isReach;

    //private TrackEntry trackEntry;

    private bool isDeath;


  

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
        //skeletonAnimation.skeleton.FindSlot(bodyPattern).SetColor(skinColor);
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

        };
    }

    private TrackEntry SetAnimation(string animationName, bool loop, float speedAnimation = 1, int trackIndex = 0)
    {
        //skeletonAnimation.state.ClearTracks();
        var anim = skeletonAnimation.state.SetAnimation(trackIndex, animationName, loop);
        anim.TimeScale = speedAnimation;
        _currentAnim = animationName;

        return anim;
    }

    private IEnumerator WalkAttack(SpiderAttackState spiderAttackState, Transform target, Vector3 dir)
    {
        isReach = false;
        _speed = speed;
        CheckFlip(dir);

        SetAnimation(walk, true, speedWalking);
        var rangeRange = Random.Range(3, 7);
        while (!isReach)
        {
            if (spiderAttackState == SpiderAttackState.MeleeAttack && Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y)) < meleeRange)
            {
                //var animMelee = SetAnimation(attack_melee, true, speedAttackMelee);
                var trackEntryMelee = SetAnimation(attack_melee, true, speedAttackMelee);
                trackEntryMelee.Complete += AnimMelee_Complete;

                isReach = true;
                break;
            }
            if (spiderAttackState == SpiderAttackState.RangeAttack && Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y)) < rangeRange)
            {
                //var animRange = SetAnimation(attack_range, true, speedAttackRange);
                var trackEntryRange = SetAnimation(attack_range, true, speedAttackRange);

                trackEntryRange.Complete += AnimRange_Complete2;
                isReach = true;
                break;
            }

            transform.Translate(dir * _speed * Time.deltaTime);
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

            //var animHurt = skeletonAnimation.state.SetAnimation(0, hurt, false);
            
            
            if (hpSlider != null)
            {
                isDeath = ChangeHp(collision.gameObject.GetComponent<TrebuchetBullet>().Damage);
            }
            if (!isDeath)
            {
                var trackEntryHurt = SetAnimation(hurt, false, speedHurt);
                trackEntryHurt.Complete += AnimHurt_Complete;
            }
            
            Destroy(collision.gameObject);
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderHurt, this, collision.gameObject.GetComponent<TrebuchetBullet>().Damage);
        }
    }

    private void AnimHurt_Complete(TrackEntry trackEntry)
    {
        SpiderWalkFromTarget();

    }

    private bool ChangeHp(int damage)
    {
        hp -= damage;
        hpSlider.value -= damage;

        if (hp <= 0)
        {
            StopAnimCoroutines();
            gameObject.layer = Layer.Dead;
            var trackEntryDeath = SetAnimation(death, false, speedDeath);
            //trackEntryDeath.TimeScale = speedDeath;
            _speed = 0;
            trackEntryDeath.Complete += AnimDeath_Complete;
            return true;
        }
        return false;
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
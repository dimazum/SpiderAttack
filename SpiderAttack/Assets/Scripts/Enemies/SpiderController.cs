using System;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpiderController : MonoBehaviour, IListener
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
    public Transform webTargetBone;
    public Transform spiderTarget;
    public Transform mainCharacter;
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




    enum SpiderAttackState
    {
        MeleeAttack,
        RangeAttack,
    }

    void Awake()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.GateDestroy, this);
        _coroutines = new List<Coroutine>();
    }


    void Start()
    {
        _speed = speed;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        //_webSlot = skeletonAnimation.skeleton.FindSlot("web");
        skeletonAnimation.skeleton.FindSlot("body_pattern1").SetColor(Color.green);

        StopAnimCoroutines();
        bool random = (Random.value < 0.5f);
        SpiderAttackState spA = random ? SpiderAttackState.RangeAttack : SpiderAttackState.MeleeAttack;
        var co = StartCoroutine(WalkLeft(SpiderAttackState.RangeAttack));
        _coroutines.Add(co);
    }

    private TrackEntry SetAnimation(string animationName, bool loop, float speedAnimation = 1, int trackIndex = 0)
    {
        var anim = skeletonAnimation.state.SetAnimation(trackIndex, animationName, loop);
        anim.TimeScale = speedAnimation;
        _currentAnim = animationName;

        return anim;
    }

    private IEnumerator WalkLeft(SpiderAttackState spiderAttackState)
    {
        _speed = speed;
        CheckFlip(Vector3.left);

        SetAnimation(walk, true, speedWalking);
        var rangeRange = Random.Range(3, 7);
        while (true)
        {
            if (spiderAttackState == SpiderAttackState.MeleeAttack && Vector3.Distance(transform.position, new Vector3(spiderTarget.position.x, transform.position.y)) < meleeRange)
            {
                var animMelee = SetAnimation(attack_melee, true, speedAttackMelee);
                animMelee.Complete += AnimMelee_Complete;

                skeletonAnimation.state.Event += delegate (TrackEntry entry, Spine.Event e)
                {
                    if (e.Data.Name == "Start_animations_folder/start_attack_melee")
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.SpiderMeleeAttackGate, this,
                            meleeDamage);
                    }
                   
                };
                break;
            }
            if (spiderAttackState == SpiderAttackState.RangeAttack && Vector3.Distance(transform.position, new Vector3(spiderTarget.position.x, transform.position.y)) < rangeRange)
            {

                webTargetBone.position = spiderTarget.position;
                var animRange = SetAnimation(attack_range, true, speedAttackRange);
                
                animRange.Complete += AnimRange_Complete;
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

                };
                break;
            }

            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            yield return null;
        }

    }

    private IEnumerator WalkRight(float time, Vector3 vector3)
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

        StopAnimCoroutines();
        bool random = (Random.value < 0.5f);
        SpiderAttackState spA = random ? SpiderAttackState.RangeAttack : SpiderAttackState.MeleeAttack;////////
        var co = StartCoroutine(WalkLeft(SpiderAttackState.RangeAttack));
        _coroutines.Add(co);
    }


    private void AnimRange_Complete(TrackEntry trackEntry)
    {
        var val = Random.value;
        var myBool = (val < .5);
        if (myBool)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkRight(Random.Range(1, 4), Vector3.right));
            _coroutines.Add(co);
        }
    }

    private void AnimMelee_Complete(TrackEntry trackEntry)
    {
        var val = Random.value;
        var myBool = (val < .5);
        if (myBool)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkRight(Random.Range(1, 4), Vector3.right));
            _coroutines.Add(co);
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
            ChangeHp(collision.gameObject.GetComponent<TrebuchetBullet>().Damage);
        }
    }

    private void AnimHurt_Complete(TrackEntry trackEntry)
    {

        if (_currentAnim == attack_melee || _currentAnim == attack_range)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkRight(Random.Range(1, 4), Vector3.right));
            _coroutines.Add(co);
        }

        if (_currentAnim == walk)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(WalkRight(Random.Range(1, 2), Vector3.right));
            _coroutines.Add(co);
        }
    }

    private void ChangeHp(int damage)
    {
        hp -= damage;
        hpSlider.value -= damage;

        if (hp <= 0)
        {
            var animWalk = skeletonAnimation.state.SetAnimation(0, death, false);
            animWalk.TimeScale = speedDeath;
            _speed = 0;
            animWalk.Complete += x => Destroy(this.gameObject);
        }
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
                spiderTarget.SetParent(mainCharacter);
                spiderTarget.position = mainCharacter.position;

                StopAnimCoroutines();
                bool random = (Random.value < 0.5f);
                SpiderAttackState spA = random ? SpiderAttackState.RangeAttack : SpiderAttackState.MeleeAttack;
                var co = StartCoroutine(WalkLeft(SpiderAttackState.RangeAttack));
                _coroutines.Add(co);

                break;

        }
    }
}
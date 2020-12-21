using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.enums;
using Spine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SpiderFlyingController : Spider
{
    private float rangeAttackRange = 5;
    //private bool isReach2 = false;

    //delegate void AnimEvent(TrackEntry entry, Spine.Event e);

    private int interval = 3;

    //void Update()
    //{
    //    if (Time.frameCount % interval == 0)
    //    {
    //        //ExampleExpensiveFunction();
    //    }
    //}

    private float timeSinceLastCalled;

    private float delay = 3f;

    //void Update()
    //{
    //    timeSinceLastCalled += Time.deltaTime;
    //    if (timeSinceLastCalled > delay)
    //    {
    //        System.GC.Collect();
    //        timeSinceLastCalled = 0f;
    //    }
    //}



    protected override IEnumerator WalkAttack(SpiderAttackType spiderAttackState, Transform target, Vector3 dir)
    {
        if (isDeath)
        {
            yield break;
        }

        float x = Random.Range(target.position.x + rangeAttackRange, target.position.x + rangeAttackRange+2);
        float y = Random.Range(0, 3.5f);
        if (_forceAttack)
        {
            y = Random.Range(0, 2f);
        }
        Vector2 target2 = new Vector2(x,y);

        var isReach2 = false;

        _speed = speed;
        CheckFlip(dir);

        SetAnimation(walk, true, speedWalking);
        //var rangeRange = Random.Range(3, 7);

        while (!isReach2 || !isReach)
        {
            float step2 = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target2, step2);
            if (Vector2.Distance(transform.position, target2) < 0.1)
            {
                isReach2 = true;
            }

           
            if (spiderAttackState == SpiderAttackType.RangeAttack && isReach2)
            {
                var animRange = SetAnimation(attack_range, true, speedAttackRange);

                animRange.Complete += AnimRange_Complete2;

                break;
            }
            yield return null;
        }
        
    }

    protected override IEnumerator WalkAway(float time, Vector3 vector3)
    {
        if (isDeath)
        {
            yield break;
        }
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
        SpiderAttackType spA = random ? SpiderAttackType.RangeAttack : SpiderAttackType.MeleeAttack;////////
        SpiderWalkToTarget();

    }

    protected IEnumerator DropDown(Vector3 vector3)
    {
        _speed = speed;
        isReach = false;
        var _speedFall = 2;
        yield return new WaitForSeconds(1);

        while (!isReach)
        {
            if (Vector2.Distance(transform.position, new Vector2(transform.position.x,-.2f)) < 0.1)
            {
                isReach = true;
            }

            transform.Translate(vector3 * _speedFall * Time.deltaTime);
            yield return null;
        }
    }

    protected override void Death()
    {
        gameObject.layer = Layer.Dead;
        var trackEntryDeath = SetAnimation(death, false, speedDeath);
        StartCoroutine(DropDown(Vector3.down));
        _speed = 0;
        trackEntryDeath.Complete += AnimDeath_Complete;
    }
}
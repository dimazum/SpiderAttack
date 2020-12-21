
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderController : Spider
{
    protected override IEnumerator WalkAttack(SpiderAttackType spiderAttackState, Transform target, Vector3 dir)
    {
        if (isDeath)
        {
            yield break;
        }

        isReach = false;
        _speed = speed;
        CheckFlip(dir);

        SetAnimation(walk, true, speedWalking);
        var rangeRange = Random.Range(3, 7);
        while (!isReach)
        {
            if (spiderAttackState == SpiderAttackType.MeleeAttack && Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y)) < meleeRange)
            {
                //var animMelee = SetAnimation(attack_melee, true, speedAttackMelee);
                var trackEntryMelee = SetAnimation(attack_melee, true, speedAttackMelee);
                trackEntryMelee.Complete += AnimMelee_Complete;

                isReach = true;
                break;
            }
            if (spiderAttackState == SpiderAttackType.RangeAttack && Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y)) < rangeRange)
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
}
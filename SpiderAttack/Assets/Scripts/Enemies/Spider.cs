using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Spider: MonoBehaviour
{
    public BattleController battleController;
    public Transform spiderTarget;
    public int rangeDamage = 50;

    void Awake()
    {
        battleController = (BattleController)FindObjectOfType(typeof(BattleController));
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

}

enum SpiderAttackState
{
    MeleeAttack,
    RangeAttack,
}

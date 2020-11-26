using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Transform spiderTarget;
    public List<Transform> allTargets;

    //public Transform FindTarget()
    //{
    //    var aim = allTargets.OrderBy(x => Vector2.Distance(transform.position, x.position)).FirstOrDefault();
    //    spiderTarget = aim;

    //    return aim;
    //}

}

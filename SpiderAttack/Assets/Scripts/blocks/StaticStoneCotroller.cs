using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticStoneCotroller : MonoBehaviour
{
    void OnBecameVisible()
    {
        SaveHelper.Instance.PutStaticBlockPosition(transform.position);
    }
}

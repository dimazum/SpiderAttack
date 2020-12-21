using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlace : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            //coll.gameObject.GetComponent<MoveController>().inBase = true;
        }
    }

    //public void OnTriggerExit2D(Collider2D coll)
    //{
    //    if (coll.tag == "player")
    //    {
    //        coll.gameObject.GetComponent<MoveController>().inBase = false;
    //    }

    //}
}

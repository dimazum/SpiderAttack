using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        { 
            GameStates.Instance.InCity = false;
            GameStates.Instance.smoothCameraSpeed = 3;

        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            GameStates.Instance.smoothCameraSpeed = 5;

        }
    }

}

using UnityEngine;

public class CaveTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        { 
            GameStates.Instance.smoothCameraSpeed = 3;
            EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);

        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            GameStates.Instance.smoothCameraSpeed = 5;
            EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);
        }
    }

}

using UnityEngine;

public class VillageTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);
        }
    }
}

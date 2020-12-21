using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateShop : MonoBehaviour
{
    // Start is called before the first frame update
    public void BuyGate()
    {

        EventManager.Instance.PostNotification(EVENT_TYPE.BuyGate, this);

    }
}

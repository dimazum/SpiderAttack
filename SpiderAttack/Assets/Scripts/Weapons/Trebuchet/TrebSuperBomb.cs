using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebSuperBomb : TrebuchetBullet
{

    public override void SpreadDamage()
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.SuperBomb, this, Damage);
    }
}

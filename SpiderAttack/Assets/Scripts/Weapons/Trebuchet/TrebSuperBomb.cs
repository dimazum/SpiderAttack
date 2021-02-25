using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebSuperBomb : TrebuchetBullet
{

    public override void SpreadDamage()
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.SuperBomb, this, new SuperBombParam()
        {
            Damage = Damage * 5, 
            position = gameObject.transform.position
        });
    }

    protected override void CheckRatingHit(Collision2D collision)
    {
    }
}

public class SuperBombParam
{
    public int Damage { get; set; }
    public Vector3 position;
}

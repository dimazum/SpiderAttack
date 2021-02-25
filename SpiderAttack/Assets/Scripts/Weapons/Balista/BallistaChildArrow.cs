using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaChildArrow : BalistaArrow
{
    public BallistaMultiArrow ballistaMultiArrow;
    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);

    public void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        if (collision.collider.CompareTag("spider"))
        {
            ballistaMultiArrow._hitSpiderCounter++;
            EventManager.Instance.PostNotification(EVENT_TYPE.ArrowHitTarget, this);
        }
        ballistaMultiArrow.ChildCounter++;
        
    }

    public void StartArrow()
    {
        gameObject.SetActive(true);
    }
}

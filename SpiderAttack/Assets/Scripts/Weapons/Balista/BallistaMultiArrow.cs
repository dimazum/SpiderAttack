using MarchingBytes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaMultiArrow : BaseArrow
{
    public BallistaChildArrow[] replicates;
    private Rigidbody2D[] replicatesRbs;
    public Vector3[] directions;
    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);
    public int speed;

    public int _childCounter;
    public int _hitSpiderCounter;

    public int ChildCounter {
        get => _childCounter;
        set {
            _childCounter = value;
            if(_childCounter > replicates.Length - 1)
            {
                EasyObjectPool.instance.ReturnObjectToPool(gameObject);
                if(_hitSpiderCounter > 0)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.ArrowHitTarget, this);
                }
                if(_hitSpiderCounter == 0)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.ArrowMissedTarget, this);
                }
            }
        }
    }

    private void Awake()
    {
        replicatesRbs = new Rigidbody2D[replicates.Length];
        for (int i = 0; i < replicates.Length; i++)
        {
            replicatesRbs[i] = replicates[i].GetComponent<Rigidbody2D>();
        }
    }

    private void Dublication()
    {
        for (int i = 0; i < replicates.Length; i++)
        {
            replicates[i].gameObject.transform.localPosition = vectorZero;
            replicates[i].gameObject.transform.localRotation = quatZero;
            _childCounter = 0;
            _hitSpiderCounter = 0;
            replicates[i].StartArrow();
            replicatesRbs[i].AddRelativeForce(directions[i] * speed);
        }
    }

    public void OnEnable()
    {
        ChildCounter = 0;
        Dublication();
    }
}

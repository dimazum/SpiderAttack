using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.enums;
using MarchingBytes;
using UnityEngine;

public class TrebuchetBullet : Bullet
{
    private EasyObjectPool _easyObjectPool;
    public Animation animation;
    public GameObject explosionObj;
    public float explosionDelay = 1f;
    public bool timerIsRunning = false;

    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);

    void Start()
    {
        _easyObjectPool = transform.parent.GetComponentInParent<EasyObjectPool>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        explosionObj.transform.SetParent(null);
        animation.Play();
        timerIsRunning = true;
        gameObject.layer = Layer.Dead;

    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (explosionDelay > 0)
            {
                explosionDelay -= Time.deltaTime;
            }
            else
            {
                _easyObjectPool.ReturnObjectToPool(gameObject);
                explosionDelay = 1f;
                timerIsRunning = false;
                BackToStartState();
            }
        }
    }

    private void BackToStartState()
    {
        gameObject.layer = Layer.BulletStone;
        gameObject.transform.localPosition = vectorZero;
        gameObject.transform.localRotation = quatZero;
        explosionObj.transform.SetParent(gameObject.transform);
        explosionObj.transform.localPosition = vectorZero;
        explosionObj.transform.localRotation = quatZero;
    }
}

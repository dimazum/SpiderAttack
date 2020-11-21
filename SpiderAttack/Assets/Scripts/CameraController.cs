using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour, IListener
{
    public Transform player;
    public Transform bulletPosition;
    public Vector3 offset;
    public Vector3 desiredPos;
    public Vector3 smoothPos;
    public float index;

    private bool _fireTrebuchet;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.TrebShot, this);
    }

    public void LateUpdate()
    {
        //if (_fireTrebuchet && bulletPosition != null)
        //{
        //    desiredPos = bulletPosition.position + offset;
        //}
        //else
        //{
            desiredPos = player.position + offset;
        //}
        
        smoothPos = Vector3.MoveTowards(transform.position, desiredPos, 5f * Time.deltaTime);
        transform.position = smoothPos;
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.TrebShot:
                if (Param != null)
                {
                    _fireTrebuchet = true;
                    bulletPosition = (Transform)Param;
                }
                break;
        }
    }
}

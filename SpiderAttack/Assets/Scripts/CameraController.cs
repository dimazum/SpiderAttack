using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour, IListener
{
    private Camera _camera;
    public Transform player;
    public Transform mainHouse;
    public Transform bulletPosition;
    public Vector3 offset;
    public Vector3 desiredPos;
    public Vector3 smoothPos;
    //public float speedOffset = 5;
    public float index;


    void Start()
    {
        _camera = Camera.main;
        EventManager.Instance.AddListener(EVENT_TYPE.TrebShot, this);
        EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CharInCity, this);
    }

    public void LateUpdate()
    {

        desiredPos = player.position + offset;
        if (transform.position != desiredPos)
        {
            smoothPos = Vector3.MoveTowards(transform.position, desiredPos, GameStates.Instance.smoothCameraSpeed * Time.deltaTime);
            transform.position = smoothPos;
        }
        
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.TrebShot:
                if (Param != null)
                {
                    bulletPosition = (Transform)Param;
                }
                break;

            case EVENT_TYPE.GameOver:
                offset = new Vector3(0, .76f, -10);
                _camera.orthographicSize = 2.5f;
                player = mainHouse;
                break;

            case EVENT_TYPE.CharInCity:

                var inCity = (bool)Param;
                if (inCity)
                {
                    offset = new Vector3(0, 1.5f, -10);
                }
                else
                {
                    offset = new Vector3(0, .6f, -10);
                }

                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 1f;
    public Vector3 desiredPos;
    public Vector3 smoothPos;
    public float index;

    //void Awake()
    //{
    //    desiredPos = new Vector3(player.position.x +offset.x, player.position.y + offset.y, player.position.z+offset.z);
    //    var screen = GetScreenWith();
    //}

    public void LateUpdate()
    {
        
        desiredPos = player.position + offset;
        smoothPos = Vector3.MoveTowards(transform.position, desiredPos, 5f * Time.deltaTime);

        transform.position = smoothPos;
    }

    int GetScreenWith()
    {
        return Screen.width;
    }

}

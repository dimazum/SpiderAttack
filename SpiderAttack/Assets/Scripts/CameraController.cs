using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 desiredPos;
    public Vector3 smoothPos;
    public float index;

    public void LateUpdate()
    {
        desiredPos = player.position + offset;
        smoothPos = Vector3.MoveTowards(transform.position, desiredPos, 5f * Time.deltaTime);
        transform.position = smoothPos;
    }
}

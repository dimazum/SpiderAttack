using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContainer : MonoBehaviour
{
    public GameObject camera;
    private CameraController cameraController;

    private Vector3 initState = new Vector3(0, 2, -10);
    void Awake()
    {
        cameraController = camera.GetComponent<CameraController>();
    }
    public void OnTriggerEnter2D(Collider2D coll)
    {
    //    if (coll.tag == "player")
    //    {
    //        cameraController.smoothSpeed = 1f;
    //    }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        //if (coll.tag == "player")
        //{
        //    cameraController.smoothSpeed = 0.05f;
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlaceSecondFloor : MonoBehaviour
{
    private CameraController cameraController;
    private Animator cameraAnimator;
    private float screen;
    public GameObject mainCamera;

    void Awake()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
        cameraAnimator = mainCamera.GetComponent<Animator>();
        screen = (float)Screen.width / 1000;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            cameraController.index = screen;
            cameraAnimator.Play("CameraFirstPlace");
            //cameraController.offset = new Vector3(3 * ((float)Screen.width / 1000), -0.5f, -10);
            cameraController.offset = new Vector3(3 , -0.5f, -10);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            cameraAnimator.Play("CameraFirstPlace0");
            cameraController.offset = new Vector3(0, 1, -10);
        }
    }
}

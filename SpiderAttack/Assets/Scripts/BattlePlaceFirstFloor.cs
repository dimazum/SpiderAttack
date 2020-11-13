using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlaceFirstFloor : MonoBehaviour
{
    private Animator cameraAnimator;
    private Animator animator;
    private CameraController cameraController;
    private float screen;
    public GameObject mainCamera;

    private Vector3 initState = new Vector3(0, 1, -10);

    void Awake()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
        cameraAnimator = mainCamera.GetComponent<Animator>();
        animator = GetComponent<Animator>();
        screen = (float)Screen.width / 1000;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            cameraController.index = screen;
            cameraAnimator.Play("CameraFirstPlace");
            animator.Play("SecondBatlePlaceDisappear");
            cameraController.offset = new Vector3(4f , .7f, -10);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            cameraAnimator.Play("CameraFirstPlace0");
            animator.Play("SecondBatlePlaceDisappear0");
            cameraController.offset = initState;
        }
    }
}

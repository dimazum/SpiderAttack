using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlace : MonoBehaviour
{
    public Animator animator;
    public GameObject camera;
    private CameraController cameraController;

    private float screen;

    private Vector3 initState = new Vector3(0, 2, -10);

    void Awake()
    {
        cameraController = camera.GetComponent<CameraController>();
        animator = camera.GetComponent<Animator>();

         screen = (float)Screen.width / 1000;
    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            Debug.Log(coll.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            cameraController.index = screen;
            animator.Play("CameraFirstPlace");
            cameraController.offset = new Vector3(3* ((float)Screen.width/1000), 1, -10);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            animator.Play("CameraFirstPlace0");
            cameraController.offset = new Vector3(0, 1, -10);
        }
    }
}

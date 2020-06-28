using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
     Animator animatorCanvas;
    public Canvas canvas;
    public CaveEntrance CE;
    public cameraController cC;
    // Use this for initialization
    public  void Start()
    {
        
        animatorCanvas = canvas.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            if(collision is BoxCollider2D)
            {
                cC.animator.Play("PortalCameraUp");
                CE.fonarik.SetActive(false);
                animatorCanvas.Play("Portal");
                // animator.Play("MountaimInvisible");
                CE.inCave = false;
            }
        }
    }

}

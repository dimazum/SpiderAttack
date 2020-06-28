using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    public Canvas canvas;
    public Animator animator;
    public Animator animatorCanvas;
    public GameObject fonarik;
    public cameraController cC;
    public GameObject Camera;
    public bool inCave = false;
    //public bool outCave = true;
    public int count = 0;


    // Use this for initialization
    private void Awake()
    {
        animatorCanvas = canvas.GetComponent<Animator>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();



    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (inCave == false)
        {
            if (coll is BoxCollider2D)
            {
                if (coll.tag == "player")
                {
                    cC.animator.Play("CameraDown");
                    fonarik.SetActive(true);
                    animatorCanvas.Play("fogOfWarIn");
                    // animator.Play("MountaimInvisible");
                    inCave = true;
                }
            }
        }
        else if (inCave == true)
        {
            if (coll is BoxCollider2D)
            {
                if (coll.tag == "player")
                {
                    cC.animator.Play("CameraUp");
                    fonarik.SetActive(false);
                    animatorCanvas.Play("fogOfWarOut");
                    // animator.Play("MountaimInvisible");
                    inCave = false;
                }
            }
        }
    }

} 
    
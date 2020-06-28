using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRight : MonoBehaviour {
    public GameObject gateRight;
    public Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="player")
        {
            animator.Play("GateOpen");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            animator.Play("GateClose");
        }
    }
}

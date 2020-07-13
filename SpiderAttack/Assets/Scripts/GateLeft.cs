using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLeft : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            animator.Play("gate_left");
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            animator.Play("gate_left0");
        }
    }
}

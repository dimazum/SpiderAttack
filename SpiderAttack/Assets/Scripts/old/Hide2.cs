using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide2 : MonoBehaviour {

    public Animator animator;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            // Debug.Log("Зашел в ворота правые");
            animator.Play("HideRightDoor");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            // Debug.Log("Вышел из ворот");
            animator.Play("HideRightDoor2");
        }
    }
}


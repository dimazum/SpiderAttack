using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Зашел в ворота левые");
        animator.Play("HideLeftDoor");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Вышел из ворот");
        animator.Play("HideLeftDoor2");
    }
}

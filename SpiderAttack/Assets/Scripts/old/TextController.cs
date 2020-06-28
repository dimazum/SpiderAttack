using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextController : MonoBehaviour {
    public Animator animator;
    public Text textAlert;
	
	void Start () {
        animator = animator.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (gameState.itemCounterAllUp > gameState.maxItemCounterAllUp)
        //{
        //    textAlert.text = "Инвентарь переполнен";
        //    animator.Play("textAlert");
        //}
        //else textAlert.text = null;

    }
}

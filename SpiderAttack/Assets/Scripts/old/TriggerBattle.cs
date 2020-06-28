using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBattle : MonoBehaviour {
    public cameraController cC;
    public bool inBattle = false;
    public TriggerBattle2 Tb2;
    public Canvas canvasMain;
    public Trebuchet Tr;
    Animator animatorCanvas;

    // Use this for initialization
    void Start () {
        animatorCanvas = canvasMain.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {

            inBattle = true;
            if (inBattle == true && Tb2.inBattle2 == false)
            {
                cC.animator.Play("CameraInBattle");
                Tr.GetComponent<Trebuchet>().enabled = true;
                animatorCanvas.Play("BattlePlace");
            }
            //SliderAttack.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            
            //SliderAttack.SetActive(false);
            inBattle = false;
            if (inBattle == false && Tb2.inBattle2 == false)
            {
                Tr.GetComponent<Trebuchet>().enabled = false;
                cC.animator.Play("CameraOutBattle");
                animatorCanvas.Play("BattlePlaceOff");
            }
        }
    }
}

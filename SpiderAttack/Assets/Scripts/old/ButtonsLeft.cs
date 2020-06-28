using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsLeft : MonoBehaviour {
    public GameObject ButtonLeft;
    public Transform[] buttons;
    //public Text[] texts;
	// Use this for initialization
	void Start () {



        Transform button0 = ButtonLeft.transform.GetChild(0);
        Transform text0 = button0.transform.GetChild(0);
        Transform button1 = ButtonLeft.transform.GetChild(1);
        Transform text1 = button1.transform.GetChild(0);
        Transform button2 = ButtonLeft.transform.GetChild(2);
        Transform text2 = button2.transform.GetChild(0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

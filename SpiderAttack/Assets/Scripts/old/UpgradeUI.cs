using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour {
    public GameObject[] panels;
    public GameObject[] backgrounds;
    public Sprite[] spritbuttons;
    public Button[] buttons;

	// Use this for initialization
	void Start () {
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        panels[2].SetActive(false);

        backgrounds[0].SetActive(false);
        backgrounds[1].SetActive(true);
        backgrounds[2].SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UseButton0()
    {
        if (panels[0].activeSelf == false)
        {
            panels[0].SetActive(true);
            panels[1].SetActive(false);
            panels[2].SetActive(false);

           
        }
        backgrounds[0].SetActive(false);
        backgrounds[1].SetActive(true);
        backgrounds[2].SetActive(true);

        buttons[0]. GetComponent<Image>().sprite = spritbuttons[0];
        buttons[1].GetComponent<Image>().sprite = spritbuttons[1];
        buttons[2].GetComponent<Image>().sprite = spritbuttons[1];
    }
    public void UseButton1()
    {
        if (panels[1].activeSelf == false)
        {
            panels[0].SetActive(false);
            panels[1].SetActive(true);
            panels[2].SetActive(false);
        }
        backgrounds[0].SetActive(true);
        backgrounds[1].SetActive(false);
        backgrounds[2].SetActive(true);

        buttons[0].GetComponent<Image>().sprite = spritbuttons[1];
        buttons[1].GetComponent<Image>().sprite = spritbuttons[0];
        buttons[2].GetComponent<Image>().sprite = spritbuttons[1];
    }
    public void UseButton2()
    {
        if (panels[2].activeSelf == false)
        {
            panels[0].SetActive(false);
            panels[1].SetActive(false);
            panels[2].SetActive(true);
        }
        backgrounds[0].SetActive(true);
        backgrounds[1].SetActive(true);
        backgrounds[2].SetActive(false);
        buttons[0].GetComponent<Image>().sprite = spritbuttons[1];
        buttons[1].GetComponent<Image>().sprite = spritbuttons[1];
        buttons[2].GetComponent<Image>().sprite = spritbuttons[0];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgrade1 : MonoBehaviour {
    public Button buttonShop;
    public GameObject ShopPanel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            buttonShop.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            buttonShop.gameObject.SetActive(false);
            if (ShopPanel.activeSelf)
            {
                ShopPanel.SetActive(false);
            }
        }
    }
    public void ShopOn()
    {
        if (gameState.isInInventory == false)
        {
            ShopPanel.SetActive(true);
            buttonShop.gameObject.SetActive(false);
        }
       
    }
    public void ShopOff()
    {
        ShopPanel.SetActive(false);
        buttonShop.gameObject.SetActive(true);
    }
}

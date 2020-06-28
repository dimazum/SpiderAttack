using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Inventary inventory;
    public GameObject invenotoryObj;
    public List<Sprite> spr;
    public GameObject ShopPanel;
    public GameObject cellContainerShop;

    public Button enterButton;

    public Text txtName;
    public Text txtPrice;
   

    public int itemCounterAll = 0;
    public bool UseInShop = false;

    public KeyCode showShop;

    

    public Transform panel;
    public Transform ramka;

    public int n = 0;//счетчик при нажатии на стелки для рамки
    public int childC2;

    void Start()
    {
        inventory = invenotoryObj.GetComponent<Inventary>();

        childC2 = cellContainerShop.transform.childCount;
        n = 0;

        //ShopPanel.SetActive(false);

        spr = new List<Sprite>();


        //for (int i = 0; i < cellContainerShop.transform.childCount; i++)
        //{

        //    spr.Add(new Sprite());


        //}
        txtName.text = null;
        //countAllItems.text = itemCounterAll.ToString() + "/" + 20;
        ramka.localPosition = Vector3.zero;
        ramka.SetParent(cellContainerShop.transform.GetChild(0));


        //for (int i = 0; i < cellContainerShop.transform.childCount; i++)
        //{

        //    Transform cell = cellContainerShop.transform.GetChild(i);
        //    Transform icon = cell.GetChild(0);
        //    Transform count = icon.GetChild(0);
        //    Image img = icon.GetComponent<Image>();

        //    img.sprite = spr[i];
        //    //здесь список иконок в магазин(префабы добавляются в скрипте инвентаря в массив)
        //    spr[i] = Resources.Load<Sprite>(inventory.productM[0].GetComponent<Item>().pathIcon);
            
        //    img.enabled = true;
        //    //if (img.sprite != null)
        //    //{
                
        //    //}
        //    //break;

        //}
    }


    void Update()
    {
        if (inventory.productM[n] != null)
        {
            txtPrice.text = inventory.productM[n].GetComponent<Item>().Price.ToString() + "$";
        }
        else txtPrice.text = "";


        if (inventory.productM[n] != null)
        {
            txtName.text = inventory.productM[n].GetComponent<Item>().nameItem;
        }
        else txtName.text = "";

        for (int i = 0; i < cellContainerShop.transform.childCount; i++)
        {

            Transform cell = cellContainerShop.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            Image img = icon.GetComponent<Image>();

            img.sprite = spr[i];
            //здесь список иконок в магазин(префабы добавляются в скрипте инвентаря в массив)
            spr[0] = Resources.Load<Sprite>(inventory.productM[0].GetComponent<Item>().pathIcon);
            spr[1] = Resources.Load<Sprite>(inventory.productM[1].GetComponent<Item>().pathIcon);
            spr[2] = Resources.Load<Sprite>(inventory.productM[2].GetComponent<Item>().pathIcon);
            spr[3] = Resources.Load<Sprite>(inventory.productM[3].GetComponent<Item>().pathIcon);



            if (img.sprite != null)
            {
                img.enabled = true;
            }
            break;

        }

        ToggleInventory();

        Transform panel2 = panel.GetChild(0 + n);
        ramka.position = panel2.position;


        //управление рамкой в инвентаре
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            RightButtonRamka();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            LeftButtonRamka();
        }
    }
    //void AddCountItemAll()
    //{
    //    itemCounterAll++;
    //    countAllItems.text = itemCounterAll.ToString() + "/" + 20;
    //}

    public void ToggleInventory()
    {
        if (Input.GetKeyDown(showShop))
        {
            if (ShopPanel.activeSelf)
            {
                n = 0;
                ShopPanel.SetActive(false);
            }
            else
            {
                ShopPanel.SetActive(true);
                n = 0;
            }
        }
    }

    public void ExitShop()
    {
        n = 0;
        ShopPanel.SetActive(false);
        enterButton.GetComponent<Image>().enabled = true;
    }
    public void EnterShop()
    {
        ShopPanel.SetActive(true);
        n = 0;
        enterButton.GetComponent<Image>().enabled = false;
    }

    public void UseShop()
    {
        UseInShop = true;
        
    }

    public void RightButtonRamka()
    {
        if (n < childC2 - 1)
        {
            n++;
            ramka.SetParent(cellContainerShop.transform.GetChild(0 + n));
            ramka.localPosition = Vector3.zero;
        }

    }
    public void LeftButtonRamka()
    {
        if (n > 0)
        {
            n--;
            ramka.SetParent(cellContainerShop.transform.GetChild(0 + n));
            ramka.localPosition = Vector3.zero;
        }

    }

}


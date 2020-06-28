using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventary : MonoBehaviour
{

    public List<Item> item;
    public List<Item> resurs;
    // public List<Sprite> spr;


    public GameObject inventory;
    public Shop shop;
    public GameObject ShopObj;
   // moveManager mM;
    public GameObject mMObj;
    public GameObject cellContainerDown;
    public GameObject cellContainerUp;
    public RaycastHit2D hit;

    public GameObject[] productM;

    public GameObject player;
    public int itemCounterAllDown ;
    public int itemCounterAllUp ;

    public KeyCode showInventory;

    public Text countAllItemsUp;
    public Text countAllItemsDown;//счетчик в инвентаре
    public Text bigPanel;//счетчик на панели быстрого доступа
    public Text txtName;

    public Transform panel;
    public Transform ramka;
    public Input RightA;
    public int n = 0;//счетчик при нажатии на стелки для рамки
    public int childC;
    public int TestCountUp;//счетчик подобранных ресурсов
    //public int maxBagDown = 10;
    //public int TestCountDown;


    void Start()
    {
        //mM = mMObj.GetComponent<moveManager>();
        //gameState.maxItemCounterAllUp = 10;
       shop = ShopObj.GetComponent<Shop>();//ссылка на Shop

        inventory.SetActive(false);
        item = new List<Item>();
        for (int i = 0; i < cellContainerDown.transform.childCount; i++)
        {
            item.Add(new Item());

        }
        for (int i = 0; i < cellContainerUp.transform.childCount; i++)
        {
            resurs.Add(new Item());

        }

        countAllItemsDown.text = itemCounterAllDown.ToString() + "/" + item.Count;

        n = 0;
        childC = cellContainerDown.transform.childCount;

        ramka.localPosition = Vector3.zero;
        ramka.SetParent(cellContainerDown.transform.GetChild(0));
    }


    void Update()
    {
        //Debug.Log(gameState.maxItemCounterAllUp);
        //AddCountItemAllDown();
        //AddCountItemAllUp();
        //TxtName();

        //Debug.Log(mM.currentBlock2);
        ToggleInventory();

        Transform panel2 = panel.GetChild(0 + n);
        ramka.position = panel2.position;

        Vector3 left = transform.TransformDirection(Vector3.left) * 1;
        //Debug.DrawRay(new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z), player.transform.localScale.x * Vector3.left * 5, Color.green);

        //RaycastHit2D hit = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + 0.5f), player.transform.localScale.x * Vector3.left, 1f);
        //if (hit.collider != null)
        
            //if (mM.Test==true)
            //{

            //    //AddCountItemAll();

            //   // AddItem(mM.currentBlock2.GetComponent<Item>());
            //// Debug.Log(hit.collider.GetComponent<Item>().id);
            //mM.Test = false;
            //}
        


        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            RightButtonRamka();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            LeftButtonRamka();
        }
    }
    public void IdentificationItem(GameObject currentBlock)
    {
        AddItem(currentBlock.GetComponent<Item>());
    }
    void TxtName()
    {
        if (item[n] != new Item())
        {
            txtName.text = item[n].GetComponent<Item>().nameItem;

        }
        else
        {
            txtName.text = null;



        }

    }
    void AddCountItemAllDown()
    {
        itemCounterAllDown = 0;//сбрасываем чтобы не прибавляло
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id != 0)
            {
                itemCounterAllDown = itemCounterAllDown + item[i].countItem;

                
                // itemCounterAllDown=item[i].countItem;
            }
        }

        countAllItemsDown.text = itemCounterAllDown.ToString() + "/" + item.Count;
    }
    void AddCountItemAllUp()
    {

        itemCounterAllUp = 0;//сбрасываем чтобы не прибавляло
        for (int i = 0; i < resurs.Count; i++)
        {
            if (resurs[i].id != 0)
            {
                itemCounterAllUp = itemCounterAllUp + resurs[i].countItem;


                // itemCounterAllDown=item[i].countItem;
            }
        }

        
        countAllItemsUp.text = itemCounterAllUp.ToString() + "/" + gameState.maxItemCounterAllUp;
    }

    void AddItem(Item currentItem)
    {

        if (currentItem.isStackable &&currentItem.resurs==false)
        {
            AddStackableItem(currentItem);
        }
        else if (currentItem.isStackable=false && currentItem.resurs == false)
        {
            AddUnStackableItem(currentItem);
        }
        else AddStackableResurs(currentItem);//!!!!!
    }


    void AddStackableResurs(Item currentItem)
    {
        //TestCountUp++;//проверить
        for (int i = 0; i < resurs.Count; i++)
        {
            if (resurs[i].id == currentItem.id)
            {
                resurs[i].countItem++;
                DisplayItems();
                //Destroy(currentItem.gameObject);
                AddCountItemAllUp();//!!!!!проверить
                return;

            }
                       
                //resurs[i] = currentItem;

                //resurs[i].countItem = 1;

                //DisplayItems();
                //currentItem.gameObject.SetActive(false);
                //AddCountItemAllUp();//!!!!!проверить
                //break;

                  
        }
        AddUnStackableResurs(currentItem);


    }
    void AddUnStackableResurs(Item currentItem)
    {

        for (int i = 0; i < resurs.Count; i++)
        {
            if (resurs[i].id == 0)
            {

                resurs[i] = currentItem;

                resurs[i].countItem = 1;

                DisplayItems();
                //currentItem.gameObject.SetActive(false);
                //Destroy(currentItem.gameObject);
                break;

            }
        }


    }
    void AddUnStackableItem(Item currentItem)
    {

        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == 0)
            {
               
                item[i] = currentItem;
               
                item[i].countItem = 1;

                DisplayItems();
               // currentItem.gameObject.SetActive(false);
                //Destroy(currentItem.gameObject);
                break;

            }
        }


    }
    void AddStackableItem(Item currentItem)
    {
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == currentItem.id)
            {
                item[i].countItem++;
                
                DisplayItems();
                //Destroy(currentItem.gameObject);
                
                return;

            }
        }
        AddUnStackableItem(currentItem);
       
    }
    private void OnDrawGizmos()
    {
        //Vector3 v = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // Gizmos.DrawRay(player.transform.position, Vector3.left);
    }
    void ToggleInventory()
    {
        if (Input.GetKeyDown(showInventory))
        {
            if (inventory.activeSelf)
            {
                n = 0;
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
                n = 0;
            }

        }
    }
    public void ToggleInventoryButton()
    {
        
        
            if (inventory.activeSelf)
            {
                n = 0;
                inventory.SetActive(false);
            gameState.isInInventory = false;
            }
            else
            {
                inventory.SetActive(true);
            gameState.isInInventory = true;
            n = 0;
            }

        
    }
    public void RightButtonRamka()
    {
        if (n < childC - 1)
        {
            n++;
            ramka.SetParent(cellContainerDown.transform.GetChild(0 + n));
            ramka.localPosition = Vector3.zero;
        }

    }
    public void LeftButtonRamka()
    {
        if (n > 0)
        {
            n--;
            ramka.SetParent(cellContainerDown.transform.GetChild(0 + n));
            ramka.localPosition = Vector3.zero;
        }

    }
    void DisplayItems()
    {
        //цикля для нижнего инвентаря
        for (int i = 0; i < item.Count; i++)
        {
            Transform cell = cellContainerDown.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);

            Text txt = count.GetComponent<Text>();


            Image img = icon.GetComponent<Image>();
            if (item[i].id != 0)
            {

                img.enabled = true;
                img.sprite = Resources.Load<Sprite>(item[i].pathIcon);
                if (item[i].countItem >= 1)
                {
                    txt.text = item[i].countItem.ToString();
                    //bigPanel.text= item[i].countItem.ToString();//счетчик на панели быстрого доступа
                }

            }
            else
            {
                img.enabled = false;
                img.sprite = null;
                txt.text = null;
            }
        }
        //цикл для верхнего инвентаря
        for (int i = 0; i < resurs.Count; i++)
        {
            Transform cell = cellContainerUp.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);

            Text txt = count.GetComponent<Text>();


            Image img = icon.GetComponent<Image>();
            if (resurs[i].id != 0)
            {

                img.enabled = true;
                img.sprite = Resources.Load<Sprite>(resurs[i].pathIcon);
                if (resurs[i].countItem > 1)
                {
                    txt.text = resurs[i].countItem.ToString();
                    


                }

            }
            else
            {
                img.enabled = false;
                img.sprite = null;
                txt.text = null;
            }
        }
    }
    public void DisplayItems2()
    {
        for (int i = 0; i < childC; i++)
        {

            Transform cell = cellContainerDown.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            Image img = icon.GetComponent<Image>();
            if (img.sprite == null)
            {
                img.enabled = true;
                img.sprite = Resources.Load<Sprite>(productM[shop.n].GetComponent<Item>().pathIcon);
                break;
            }

        }

    }
    public void AddItemInShop()
    {
        
        for (int i = 0; i < item.Count; i++)
        {
            //if (itemCounterAllDown < maxBagDown)
            //если он есть в инвентаре то добаляем индекс +1
            if (item[i] == productM[shop.n].GetComponent<Item>())
            {
                if (itemCounterAllDown < gameState.maxItemCounterAllUp)
                {
                    //gameState.currentPlayer.money--;
                    gameState.currentPlayer.money = gameState.currentPlayer.money - productM[shop.n].GetComponent<Item>().Price;
                    item[i].countItem++;
                    
                }
               // Debug.Log(item[i].countItem + "всех итемов");
                DisplayItems();

                return;
            } 
            
        }
        //если нет то добавляем в лист
            for (int i = 0; i < item.Count; i++)
                if (item[i].id == 0)
                {
                if (itemCounterAllDown < gameState.maxItemCounterAllUp)
                {
                    item[i] = productM[shop.n].GetComponent<Item>();
                    // gameState.currentPlayer.money--;
                    gameState.currentPlayer.money= gameState.currentPlayer.money - productM[shop.n].GetComponent<Item>().Price;
                }
                    item[i].countItem = 1;
                    DisplayItems();
                    break;
                }
        
    }
    public void UseInventory()
    {

        for (int i = 0; i < item.Count; i++)
        {
           if (item[n].id != 0)
            {
                if (item[n].countItem > 1)
                {

                    item[n].countItem--;
                    
                    

                    DisplayItems();
                }
                else
                {
                    item[n] = new Item();
                    
                }
                DisplayItems();
                break;
            }
        }
        //AddCountItemAllDown();
        //AddCountItemAllUp();
    }
}

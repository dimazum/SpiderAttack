using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;

public class shopBorderController : MonoBehaviour
{

    //bool isInShop;

    public Image itemImage;
    public Text itemDescript;
    public Text itemName;
    public Text countText;
    public Text moneyText;
    public Text costText;
    public Animator animator;
    public Canvas canvas;

    public Button shopButton;

   // public Canvas canv;

    int index; //текущий элемент в продаже


    public Transform shopPanel;

    public static string shopAssets;
    public InventoryItemContainer data;


    // Use this for initialization
    void Start () {

        gameState.screenHieght = Screen.height; //получаем высоту экрана
        gameState.screenWidth = Screen.width; //получаем ширину экрана
        gameState.isInShop = false; //изначально мы не в магазине
        index = 0; //нулевой индекс элемента магазина
        shopButton.gameObject.SetActive(false); //выключаем анимацию кнопки

        //путь к файлу с описанием магазина
        shopAssets = Application.dataPath + "/data/shop.xml";
        if (File.Exists(shopAssets))
        {
            //объявляем, что мы пытаемся сериализовать наш объект по классу InventoryItemContainer
            var ser = new XmlSerializer(typeof(InventoryItemContainer));

            //читаем наш файлик-магазин
            var stream = new FileStream(shopAssets, FileMode.Open);

            //пытаемся десериализовать
            data = ser.Deserialize(stream) as InventoryItemContainer;

            //Debug.Log(data.shopItems[0].itemName + " " + data.shopItems[1].itemName);

            //int count = data.shopItems.Count; //количество товаров в магазине


            ShowItem(index);
         
        }
        else
            Debug.Log("Файл с магазином не существует!");
    }
	

    void OnTriggerEnter2D(Collider2D coll)
    {


        //canvas.GetComponent<Animator>().Play("fogOfWarIn");

        if (coll.tag == "player")
        {

            //canv.GetComponent<GraphicRaycaster>().enabled = false;
            shopButton.gameObject.SetActive(true);//включаем анимацию кнопки
            gameState.isInShop = true;//мы в магазине

            int addMoney = 0;//сумма сданных ресурсов
            foreach (var item in gameState.currentPlayer.bagDictionary)
            {
                addMoney += item.Value * gameState.resursDictionary[item.Key];
            }

            //если что-то сдали, то очицаем сумку и добавляем деньги
            int addHealth = 100 - gameState.currentPlayer.Health;
            if (addMoney != 0 || addHealth != 0)
            {
                gameState.currentPlayer.bagDictionary.Clear();
                StartCoroutine(Money(addMoney, addHealth));
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {

        //canvas.GetComponent<Animator>().Play("fogOfWarOut");

        if (coll.tag == "player")
        {
            
            shopButton.gameObject.SetActive(false); //выключаем анимацию кнопки
            gameState.isInShop = false; //мы вышли из магазина
        }
    }

    void ShowItem(int index)
    {
        InventoryItem item = data.shopItems[index];
        itemImage.sprite = Resources.Load<Sprite>(item.icon); //достаем иконку
        itemName.text = item.itemName; //достаем название элемента
        costText.text = "Цена: " + item.cost.ToString(); //достаем цену элемента
        moneyText.text = "Деньги: "+gameState.currentPlayer.money.ToString();

        if (gameState.currentPlayer.shopDictionary.ContainsKey(item.itemName))
        {
            countText.text = "Количество в рюкзаке: " + gameState.currentPlayer.shopDictionary[item.itemName]+".";
        } else countText.text = "Количество в рюкзаке: 0.";
    }

    public void Left()
    {
        index--;
        if (index < 0)
        {
            index = data.shopItems.Count-1;
        }
        ShowItem(index);
    }

    public void Right()
    {
        index++;
        if (index > data.shopItems.Count-1)
        {
            index = 0;
        }
        ShowItem(index);
    }

    public void Buy()
    {
        InventoryItem item = data.shopItems[index];
        if ((gameState.currentPlayer.money - item.cost) >= 0)
        {
            gameState.currentPlayer.money -= item.cost;
            if (gameState.currentPlayer.shopDictionary.ContainsKey(item.itemName))
            {
                gameState.currentPlayer.shopDictionary[item.itemName]++;
            }
            else
            {
                gameState.currentPlayer.shopDictionary.Add(item.itemName, 1);
            }
        }else
        {
            Debug.Log("Не хватает денег!");
        }
        ShowItem(index);

    }

    IEnumerator Money(int _addMoney, int _addHealth)
    {
        while (_addMoney>0 || _addHealth > 0)
        {
            yield return new WaitForSeconds(0);
            _addMoney--;
            if (_addMoney>=0) gameState.currentPlayer.money++;
            _addHealth--;
            if (_addHealth>=0) gameState.currentPlayer.Health++;
        }
        //Debug.Log(gameState.currentPlayer.money + " "+ gameState.currentPlayer.Health);
    }

    //private IEnumerator __Down(Transform obj, float distance)
    //{
    //    float yend = obj.position.y - distance;
    //    while ((obj.position.y - yend) > 0.0001f)
    //    {
    //        Vector3 v = obj.position;
    //        v.y -= 4.0f;
    //        obj.position = v;
    //        yield return new WaitForSeconds(0.003F);
    //    }
    //    Vector3 vend = obj.position;
    //    vend.y = yend;
    //    obj.position = vend;
    //}

    //private IEnumerator __Up(Transform obj, float distance)
    //{
    //    float yend = obj.position.y + distance;
    //    while ((yend - obj.position.y) > 0.0001f)
    //    {
    //        Vector3 v = obj.position;
    //        v.y += 4.0f;
    //        obj.position = v;
    //        yield return new WaitForSeconds(0.003f);
    //    }
    //    Vector3 vend = obj.position;
    //    vend.y = yend;
    //    obj.position = vend;
    //}

    public void shopButtonClick()
    {
        Debug.Log("Нажал!");
        
        canvas.GetComponent<Animator>().Play("shopPanel_in");
        animator.Play("ButtonHide");
        //shopButton.GetComponent<Image>().enabled = false;
        //canv.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void shopExitClick()
    {
        canvas.GetComponent<Animator>().Play("shopPanel_out");
        shopButton.GetComponent<Image>().enabled = true;
        animator.Play("ButtonHideOff");


    }




}

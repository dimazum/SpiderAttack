using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class Bag : MonoBehaviour {


//    public GameObject bag; //панель с инветарем
//    public bool isBag;

//    public GameObject cont;
//    public GameObject text;
//    public GameObject itemPanel;

//    public Transform Hero;      //ссылка на трансформ героя
//    public GameObject support;  //ссылка на опору
//    public GameObject stairs;  //ссылка на лестницу
//    public GameObject bomb;


//    // Use this for initialization
//    void Start () {
//        bag.SetActive(false);
//        isBag= false;
//    }
	
//	// Update is called once per frame
//	void Update () {

//        if (Input.GetKeyDown(KeyCode.I))
//        {
//            if (bag.activeSelf)
//            {
//                bag.SetActive(false);
//                ClearPanel();
                
//            }
//            else
//            {
//                bag.SetActive(true);
//                CreatePanel();
                
//            }
//        }
//    }

//    void remove(string item)
//    {
//        if (gameState.currentPlayer.shopDictionary.ContainsKey(item))
//        {
//            bool flag = false;
//            switch (item)
//            {
//                case "Support":
//                    flag = CreateSupport();
//                    break;
//                case "Stairs":
//                    flag = CreateStairs();
//                    break;
//                case "Bomb":
//                    flag = CreateBomb();
//                    break;

//                default:
//                    Debug.Log("Отсутствует элемент сумки для обработки!");
//                    break;
//            }

//            //если все удачно, уменьшаем количество элементов из сумки и перерисовываем панель
//            if (flag)
//            {
//                gameState.currentPlayer.shopDictionary[item]--;
//                if (gameState.currentPlayer.shopDictionary[item] <= 0)
//                {
//                    gameState.currentPlayer.shopDictionary.Remove(item);
//                }
//                ClearPanel();
//                CreatePanel();
//            }
//        }
//    }

//    void ClearPanel()
//    {
//        for (int j = 0; j < bag.transform.childCount; j++)
//        {
//            if (bag.transform.GetChild(j).transform.childCount > 0)
//            {
//                Destroy(bag.transform.GetChild(j).gameObject);
//            }
//        }
//    }

//    void CreatePanel()
//    {
//        int i = 0;
//        //пробегаемся по собранным ресурсам
//        foreach (var item in gameState.currentPlayer.bagDictionary)
//        {
//            GameObject _itemPanel = Instantiate(itemPanel);
//            _itemPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("icon" + item.Key);
//            _itemPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = item.Value.ToString();
//            _itemPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "";
//            _itemPanel.transform.SetParent(bag.transform);
//            _itemPanel.transform.localScale = new Vector3(1, 1, 1);
//            i++;
//        }
//        //пробираемся по инвентарю
//        foreach (var item in gameState.currentPlayer.shopDictionary)
//        {
//            GameObject _itemPanel = Instantiate(itemPanel);
//            _itemPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.Key + "_icon");
//            _itemPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = item.Value.ToString();
//            _itemPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "";
//            _itemPanel.transform.SetParent(bag.transform);
//            _itemPanel.transform.localScale = new Vector3(1, 1, 1);

//            _itemPanel.AddComponent<Button>().onClick.AddListener(() => remove(item.Key));
//            i++;
//        }
//    }

//    //установка бомбы
//    bool CreateBomb()
//    {
//        Vector2 mousePos;
//        mousePos.x = Mathf.Round(Hero.position.x);
//        mousePos.y = Mathf.Round(Hero.position.y);

//        bomb.transform.position = mousePos;//перемистили бомбу в точку;
//        bomb.GetComponent<Animator>().SetTrigger("runBomb");

//        Collider2D[] colladers = Physics2D.OverlapCircleAll(mousePos, 1.1f, 1 << 9);
//        StartCoroutine(BombDestroy(colladers, mousePos));

//        return true;
//    }

//    IEnumerator BombDestroy(Collider2D[] colladers, Vector2 mousePos)
//    {
//        //делаем задержку для анимации бомбы
//        yield return new WaitForSeconds(1.2f);
//        GameObject currentGO;

//        //подгружаем спрайты дестроя
//        for (int i = 0; i < gameState.destroySp.Length; i++)
//        {
//            foreach (Collider2D item in colladers)
//            {
//                currentGO = item.gameObject;
//                currentGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = gameState.destroySp[i];//спрайт для дестроя
//            }
//            yield return new WaitForSeconds(0.09f);
//        }

//        //"уничтожаем" блоки
//        foreach (Collider2D item in colladers)
//        {
//            currentGO = item.gameObject;
//            currentGO.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;//спрайт фон
//            currentGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;//спрайт для дестроя
//            currentGO.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;// спрайт ресурсов
//            currentGO.layer = 10; //перемещаем блок на мертвый слой, с которым у игрока нет физической коллизии;
//        }

//        //движение того, что сверху после взрыва
//        //указатель в верхний левый угол блока из девяти элментов
//        mousePos.x = mousePos.x - 1;
//        mousePos.y = mousePos.y + 1;
//        for (int i = 0; i < 3; i++)
//        {
//            Vector2 pos = mousePos;
//            Vector2 direction = Vector2.up;
//            float distance = 0.6f;
//            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9); //стреляем вверх
//            if (hit.collider != null)
//            {
//                if (hit.collider.tag != "blockGround") //если попали не в землю, то двигаем то что свреху
//                {
//                    IMoveDown gm = hit.collider.GetComponent<IMoveDown>();
//                    if (hit.collider.tag == "stoneDynamic") //если попали в камень, то вызваем дрожание камеры и движение блок с задержкой
//                    {
//                        Debug.Log("Я взорванный " + i + "-й, пытаюсь двигать блок!");
//                        gm.MoveObjectDown(2f);
//                    }
//                    else
//                    {
//                        Debug.Log("Я взорванный " + i + "-й, пытаюсь двигать, то что сверху!");
//                        gm.MoveObjectDown(0);
//                    }
//                }
//            }
//            mousePos.x++;
//        }
//    }

//    //установка опоры
//    bool CreateSupport()
//    {
//        bool isCreated = false;
//        //установка опоры по трансформу героя
//        float posX = Mathf.Round(Hero.position.x);
//        float posY = Mathf.Round(Hero.position.y) + 1;
//        Vector2 pos = new Vector2(posX, posY);

//        RaycastHit2D InstHit = Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9); //пускаем луч по слою блоков
//        if (InstHit.collider == null)
//        {
//            pos.y = pos.y - 1;
//            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9).collider == null)
//            {
//                Debug.Log("Не могу поставить опору в воздухе!");
//            }
//            else
//            {
//                if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9).collider.tag == "stairs")
//                {
//                    Debug.Log("Не могу поставить опору на лестнице!");
//                }
//                else
//                {
//                    Debug.Log("Место свободно, могу поставить опору!");
//                    Instantiate(support, pos, Quaternion.identity); //если ничего не нашли, то ставим опору
//                    isCreated = true;
//                }
//            }
//        }
//        else Debug.Log("Место занято, не могу поставить опору!");
//        return isCreated;
//    }

//    //установка лестницы
//    bool CreateStairs()
//    {
//        bool isCreated = false;
//        float posX = Mathf.Round(Hero.position.x);
//        float posY = Mathf.Round(Hero.position.y) + 1;
//        Vector2 pos = new Vector2(posX, posY);

//        RaycastHit2D InstHit = Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9);//пускаем луч вниз с единицу выше по слою блоков

//        if (InstHit.collider == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
//        {
//            pos.y = posY - 1; //возвращаемся в центр установки
//            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9).collider == null)//стреляем вниз
//            {
//                Debug.Log("Не могу поставить лестницу в воздухе!");
//            }
//            else
//            {
//                isCreated = true;
//                Debug.Log("Место свободно, могу поставить лестницу!");
//                Instantiate(stairs, pos, Quaternion.identity); //если ничего не нашли внизу не пусто, то ставим лестницу
//            }
//        }
//        else
//        {
//            Debug.Log("Место занято, не могу поставить лестницу! Стоит: " + InstHit.collider.tag);
//        }
//        return isCreated;
//    }
//}

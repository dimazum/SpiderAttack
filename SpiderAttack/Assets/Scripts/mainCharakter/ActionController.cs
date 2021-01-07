using Assets.Scripts.enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public GameObject ladder;
    public GameObject bomb;
    public Transform sceneContainer;
    public Transform camera;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Teleport();
        }
    }

    public void SetLadder()
    {
        float posX;
        if (transform.position.x > 0)
        {
            posX = (int)transform.position.x + 0.5f;
        }
        else
        {
            posX = (int)transform.position.x - 0.5f;
        }

        float posY = Mathf.Round(transform.position.y) + 0.5f;
        Vector2 pos = new Vector2(posX, posY);

        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << Layer.Ladders);

        RaycastHit2D InstHit = Physics2D.Raycast(pos, Vector2.down, 1f, Layer.Blocks | 1 << Layer.Ladders);

        if (check == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
        {
            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones).collider != null)//стреляем вниз
            {
                Instantiate(ladder, pos, Quaternion.identity, sceneContainer); //если ничего не нашли внизу не пусто, то ставим лестницу
                EventManager.Instance.PostNotification(EVENT_TYPE.SetLadder, this);
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        }
    }

    public void Teleport()
    {
        camera.SetParent(gameObject.transform);
        transform.position = new Vector3(-18, 0, 0);
        camera.SetParent(null);
        EventManager.Instance.PostNotification(EVENT_TYPE.Teleport, this);
    }

    public void SetBomb()
    {
        float posX;
        int index = transform.position.x > 0? 1:-1;
        if(transform.localScale.x < 0)
        {
            posX =  (int)transform.position.x + (index * 0.5f) + 0.3f;
        }
        else
        {
            posX =  (int)transform.position.x + (index* 0.5f) - 0.3f;
        }
       


        float posY = Mathf.Round(transform.position.y) + 0.3f;
        Vector2 pos = new Vector2(posX, posY);

        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << Layer.Ladders);


        if (check == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
        {
            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones).collider != null)//стреляем вниз
            {
                var bombObj = Instantiate(bomb, pos, Quaternion.identity, sceneContainer); //если ничего не нашли внизу не пусто, то ставим лестницу
                //EventManager.Instance.PostNotification(EVENT_TYPE.SetLadder, this);
                Vector2 explosionDir = new Vector2(transform.localScale.x, 0);
                StartCoroutine(BombDestroy(pos, bombObj, explosionDir));
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        }

        
    }

    public void Dinamit()
    {
        float posX = Mathf.Round(transform.position.x);
        float posY = Mathf.Round(transform.position.y);
        Vector2 pos = new Vector2(posX, posY);

        bomb.transform.position = pos;//перемистили бомбу в точку;
        bomb.GetComponent<Animator>().SetTrigger("runBomb");

        Collider2D[] colladers = Physics2D.OverlapCircleAll(pos, 1.1f, 1 << 9);
        //StartCoroutine(BombDestroy(colladers, pos));
    }

    IEnumerator BombDestroy( Vector2 mousePos, GameObject bomb, Vector2 explosionDir)
    {
        yield return new WaitForSeconds(1.2f);
        GameObject block = Physics2D.Raycast(mousePos, -explosionDir, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones).collider?.gameObject;
        if (block != null) {
           Destroy(block);
        }
        Destroy(bomb);

        ////делаем задержку для анимации бомбы
        //yield return new WaitForSeconds(1.2f);
        //GameObject currentGO;

        ////подгружаем спрайты дестроя
        //for (int i = 0; i < destroySp.Length; i++)
        //{
        //    foreach (Collider2D item in colladers)
        //    {
        //        currentGO = item.gameObject;
        //        currentGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = destroySp[i];//спрайт для дестроя
        //    }
        //    yield return new WaitForSeconds(0.09f);
        //}

        ////"уничтожаем" блоки
        //foreach (Collider2D item in colladers)
        //{
        //    currentGO = item.gameObject;
        //    currentGO.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;//спрайт фон
        //    currentGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;//спрайт для дестроя
        //    currentGO.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;// спрайт ресурсов
        //    currentGO.SetActive(false);
        //    //currentGO.GetComponent<BoxCollider2D>().isTrigger = true;
        //    //currentGO.layer = 10; //перемещаем блок на мертвый слой, с которым у игрока нет физической коллизии;
        //    //Destroy(item);
        //}

        ////движение того, что сверху после взрыва
        ////указатель в верхний левый угол блока из девяти элментов
        //mousePos.x = mousePos.x - 1;
        //mousePos.y = mousePos.y + 1;
        //for (int i = 0; i < 3; i++)
        //{
        //    Vector2 pos = mousePos;
        //    Vector2 direction = Vector2.up;
        //    float distance = 0.6f;
        //    RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9); //стреляем вверх
        //    if (hit.collider != null)
        //    {

        //        if (hit.collider.name.Contains("Ground")) //если попали не в землю, то двигаем то что свреху
        //                                                  //    if (hit.collider.tag != "blockGround")
        //        {
        //            IMoveDown gm = hit.collider.GetComponent<IMoveDown>();
        //            //if (hit.collider.tag == "stoneDynamic") //если попали в камень, то вызваем дрожание камеры и движение блок с задержкой
        //            if (hit.collider.name.Contains("stone"))
        //            {
        //                Debug.Log("Я взорванный " + i + "-й, пытаюсь двигать блок!");
        //                gm.MoveObjectDown(2f);
        //            }
        //            else
        //            {
        //                //Debug.Log("Я взорванный " + i + "-й, пытаюсь двигать, то что сверху!");
        //                gm.MoveObjectDown(0);
        //            }
        //        }
        //    }
        //    mousePos.x++;
        //}
    }

}

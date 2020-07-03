using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class stairsController : MonoBehaviour, IMoveDown
{

    public float speed;
    public float delay;

    

    public void OnTriggerStay2D(Collider2D coll)
    {

        if (coll is CircleCollider2D && coll.tag == "player")
        {
           // coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 4f;

            //coll.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }


    public void OnTriggerExit2D(Collider2D coll)
    {

        if (coll is CircleCollider2D && coll.tag == "player")
        {
            //coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 2f;
            //coll.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }


    public void MoveObjectDown(float delayTime)
    {
        delay = delayTime;
        StartCoroutine(Down());
    }


    private IEnumerator Down()
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Я лестница! Пытаюсь двигаться!");


        Vector2 pos = transform.position;
        Vector2 direction = Vector2.up;
        float distance = 0.6f;
        //стреляем вверх и запоиманем в hit_up что сверху
        RaycastHit2D hit_up = Physics2D.Raycast(pos, direction, distance, 1 << 9);

        bool trigger;
        do
        {
            trigger = false; //изначально проверка не нужна
            pos = transform.position;
            direction = Vector2.down;
            distance = 0.6f;
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9); //стреляю вниз
            if (hit.collider == null) //если внизу свободно, то двигаюсь вниз
            {
                Debug.Log("Я лестница! Двигаюсь вниз! Свободно!");
                transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
                trigger = true;  //нужная еще одна проверка
            }
            else
            {
                Debug.Log("Я лестница! Не могу падать, подомной стоит " + hit.collider.tag);
            }
        } while (trigger);


        //если сверху была не земля, то двигаем то что сверху вниз
        if (hit_up.collider != null)
            if (hit_up.collider.tag != "blockGround")
            {
                IMoveDown gm = hit_up.collider.GetComponent<IMoveDown>();
                gm.MoveObjectDown(0);
            }

    }

}

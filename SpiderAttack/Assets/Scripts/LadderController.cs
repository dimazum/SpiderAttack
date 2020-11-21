using Assets.Scripts.enums;
using System.Collections;
using UnityEngine;

public class LadderController : MonoBehaviour, IMoveDown
{

    public float speed;
    public float delay;

    

    public void OnTriggerStay2D(Collider2D coll)
    {

    }


    public void OnTriggerExit2D(Collider2D coll)
    {

    }


    public void MoveObjectDown()
    {
        //delay = delayTime;
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
        RaycastHit2D hit_up = Physics2D.Raycast(pos, direction, distance, 1 << Layer.Ladders);

        bool trigger;
        do
        {
            trigger = false; //изначально проверка не нужна
            pos = transform.position;
            direction = Vector2.down;
            distance = 0.6f;
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << Layer.Ladders | 1 << Layer.Blocks); //стреляю вниз
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
            if (!hit_up.collider.name.Contains("blockGround"))
            {
                IMoveDown gm = hit_up.collider.GetComponent<IMoveDown>();
                gm.MoveObjectDown();
            }

    }

}

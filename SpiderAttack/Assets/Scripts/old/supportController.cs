using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class supportController : MonoBehaviour, IMoveDown {

//    public float delay;
//    public int NumberOfScene { get; set; }


//    public void MoveObjectDown(float delayTime)
//    {
//        delay = delayTime;
//        StartCoroutine(Down());



//    }

//    private IEnumerator Down()
//    {
//        yield return new WaitForSeconds(delay);

//        Debug.Log("Я поддержка! Пытаюсь двигаться!");


//        Vector2 pos = transform.position;
//        Vector2 direction = Vector2.up;
//        float distance = 0.6f;
//        //стреляем вверх и запоиманем в hit_up что сверху
//        RaycastHit2D hit_up = Physics2D.Raycast(pos, direction, distance, 1 << 9);

//        bool trigger;
//        do
//        {
//            trigger = false; //изначально проверка не нужна
//            pos = transform.position;
//            direction = Vector2.down;
//            distance = 0.6f;
//            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9); //стреляю вниз
//            if (hit.collider == null) //если внизу свободно, то двигаюсь вниз
//            {
//                Debug.Log("Я поддержка! Двигаюсь вниз! Свободно!");
//                transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
//                trigger = true;  //нужная еще одна проверка
//            }
//            else
//            {
//                Debug.Log("Я поддержка! Не могу падать, подомной стоит " + hit.collider.tag);
//                if (hit.collider.tag == "stairs")
//                {
//                    Destroy(hit.collider.gameObject); //если лестница, то уничтожаем её
//                    transform.position = new Vector2(transform.position.x, transform.position.y - 1f);  //вдигаемся вниз
//                    trigger = true;  // нужна еще проверка
//                }
//            }
//        } while (trigger);


//        //если сверху была не земля, то двигаем то что сверху вниз
//        if (hit_up.collider != null)
//            if (hit_up.collider.tag != "blockGround")
//            {
//                IMoveDown gm = hit_up.collider.GetComponent<IMoveDown>();
//                gm.MoveObjectDown(0);
//            }


//    }

//    //public void MoveSupportDown()
//    //{
//    //    Vector2 pos = transform.position;
//    //    Vector2 direction = Vector2.up;
//    //    float distance = 0.6f;
//    //    RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9);

//    //    direction = Vector2.down;
//    //    while (Physics2D.Raycast(pos, direction, distance, 1 << 9).collider == null)
//    //    {
//    //        //Debug.Log("Я лестница, подомной пусто, двигаюсь вниз!");
//    //        transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
//    //        pos = transform.position;
//    //    }

//    //    //стреляем вниз, если лестница, то уничтожаем ее
//    //    direction = Vector2.down;
//    //    RaycastHit2D hit2;
//    //    bool trigger = false;

//    //    do
//    //    {
//    //        trigger = false; //предпологаем что проверка не нужна
//    //        pos = transform.position;  //обновляем позицию

//    //        hit2 = Physics2D.Raycast(pos, direction, distance, 1 << 9);  //стреляем вниз
//    //        if (hit2.collider != null) //если попали, проверяем, куда попали
//    //        {
//    //            if (hit2.collider.tag == "stairs")
//    //            {
//    //                Destroy(hit2.collider.gameObject);               //если лестница, то уничтожаем её
//    //                transform.position = new Vector2(transform.position.x, transform.position.y - 1f);  //вдигаемся вниз
//    //                trigger = true;  // нужна еще проверка
//    //            }
//    //        }
//    //        else //если никуда не попали
//    //        {
//    //            transform.position = new Vector2(transform.position.x, transform.position.y - 1f); //если никуда не попали, то двигаемся вниз
//    //            trigger = true; //нужна еще проверка
//    //        }


//    //    } while (trigger); //стреляем, если пу


//    //    //если сверху опоры камень или другая опора, то двигаем их
//    //    if (hit.collider != null)
//    //    {
//    //        if (hit.collider.tag == "stoneDynamic")
//    //        {
//    //            hit.collider.GetComponent<stoneDynamic>().MoveBlockDown2();
//    //        }
//    //        if (hit.collider.tag == "support")
//    //        {
//    //            hit.collider.GetComponent<supportController>().MoveSupportDown();
//    //        }

//    //    }
//    //}
//}

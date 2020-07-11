using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class blockGround : MonoBehaviour,IMoveDown {

//	public int count;                       //счетчик ударов по блоку
//    public SpriteRenderer destroySpriteR;   //сслыка для спрайдов разрушения
//    public SpriteRenderer fon;              //ссылка на фон (земля)
//    public SpriteRenderer resource;         //ссылка на ресурс
//    public SpriteRenderer sopli;
//    public float delay;
//    //Animator
//    //public int NumberOfScene { get; set; }

//    void Start ()
//    {
//        count = 0;
//	}

//    public void MoveObjectDown(float delayTime )
//    {
//        delay = delayTime;
//        StartCoroutine(Down());
//    }

//    private IEnumerator Down()
//    {

//        yield return new WaitForSeconds(delay);

//        Vector2 pos = transform.position;
//        Vector2 direction = Vector2.up;
//        float distance = 0.6f;
//        RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9); //стреляем вверх
//        if (hit.collider != null)
//        {
//            //Debug.Log("popal v!!!");
//            if (!hit.collider.name.Contains("Ground")) //если попали не в землю, то двигаем то что свреху
//            {
//                //Debug.Log("попал не в землю");
//                IMoveDown gm = hit.collider.GetComponent<IMoveDown>();
//                //if (hit.collider.tag == "stoneDynamic") //если попали в камень, то вызваем дрожание камеры и движение блок с задержкой
//                if (hit.collider.name.Contains("stone"))
//                {
//                    //Debug.Log("Я земля, пытаюсь двигать блок!");
//                    gm.MoveObjectDown(2f);
//                }
//                else
//                {
//                    //Debug.Log("Я земля, пытаюсь двигать, то что сверху!");
//                    gm.MoveObjectDown(0);
//                }
//            }
//        }
//    }



//}

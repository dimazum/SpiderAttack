using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharState
{
    Idle,
    Run,
    Rubilovo,
    IdleBlink
}


public class MoveManager : MonoBehaviour
{
    public Text text;

    


    private Rigidbody2D rb;
    private float move;                   //перемещение из инпута
    public bool richtDirect;         //направление
    public float speed = 2f;                  //скорость
    RaycastHit2D hit;
    public float horizontalRayRange = 0.4f;

    public int count;
    public int count1;



    public GameObject explosion; //анмация хватания объекта

    public GameObject stairs;  //ссылка на лестницу
    public GameObject support; //ссылка на опору
    public GameObject bomb;//ссылка на бомбу


    public Sprite[] destroySp;
    public Sprite[] backgroundSp;
    public Sprite[] snotSp;




    Animator animator;
    private bool isGrounded; //нахождение на земле
    private bool canUP;      //возможность ползти по лестнице
    private bool canMove;
    public bool canMoveUp; //есть ли сверху препятствие
    private bool underMeStairs;

    public CharState state
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }


    void Awake()
    {
        //Application.targetFrameRate = -1;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        richtDirect = false;

        isGrounded = false;
        canUP = false;
        canMove = false;
    }


    void Update()
    {

        if (Input.GetButton("Horizontal"))
        {
            move = Input.GetAxisRaw("Horizontal");

            if (canMove)
            {
                Vector2 vector2 = transform.right * move;
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + vector2, speed * Time.deltaTime);
                state = CharState.Run;
            }

            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right * move, horizontalRayRange, 1 << 10);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right * move, Color.red, horizontalRayRange);


            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("block"))
                {
                    canMove = false;
                    state = CharState.Rubilovo;
                }
            }
            else
            {
                canMove = true;
            }

            if (move > 0 && !richtDirect) Flip();
            if (move < 0 && richtDirect) Flip();
        }

        else if (Input.GetButton("Vertical"))
        {
            



            move = Input.GetAxisRaw("Vertical");

            if (canMoveUp)
            {
                //Vector2 vector2 = transform.up * move;
                //transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + vector2, speed * Time.deltaTime);
                state = CharState.Run;
            }

            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up * move, .6f, 1 << 10);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up * move, Color.red, horizontalRayRange);

            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("block"))
                {
                    canMove = false;
                    state = CharState.Rubilovo;
                }
            }
            else
            {
                canMove = true;
            }
        }

        else
        {
            state = CharState.IdleBlink;
        }



        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.0f), 1 << 12);


        if (check != null && check.tag == "stairs")
        {
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 4f;
            canMoveUp = true;
            if (Input.GetButton("Vertical"))
            {
                float move = Input.GetAxisRaw("Vertical");
                Debug.Log(move);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, move * speed * 1.3f);
            }
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.785f);




        }

    }

    public void HitBlock()
    {
        GameObject currentblock = hit.collider?.gameObject;
        if (currentblock != null)
        {
            BlockGroundDefault blockGroundDefault = currentblock.GetComponent<BlockGroundDefault>();
            blockGroundDefault?.Hit();

            if (blockGroundDefault != null)
            {
                blockGroundDefault.Notify += message => { count++; };
            }
            count1 = count/3;
            text.text = count1.ToString();

        }
    }

    void Flip()
    {
        richtDirect = !richtDirect;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(transform.position.x, transform.position.y - 0.0f), .025f);
    }

}

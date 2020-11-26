using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.enums;
using UnityEngine;
using UnityEngine.UI;

public enum CharState
{
    Idle,
    Walk,
    Rubilovo,
    IdleKirche = 4,
    WalkKirche = 6,
    //IdleBlink
}


public class MoveController : MonoBehaviour, IListener
{
    public Text text;
    public int count;
    public int count1;

    private Rigidbody2D rb;
    private float move;                   //перемещение из инпута
    public bool richtDirect;         //направление
    public float speed = 2f;                  //скорость
    RaycastHit2D hit;
    private float horizontalRayRange = 0.4f;
    private float verticalRayRange = 0.6f;

    float? temp = null;

    public bool test = true;
    private bool isLadder = false;

    public GameObject ladder;
    public Transform sceneContainer;

    public FixedJoystick fixedJoystick;

    Animator animator;
    public bool blockMove = false;
    public bool canMove;
    public bool canMoveUp; //есть ли сверху препятствие
    public bool inBase = true;

    //kirche lvl
    public int kircheLvl;

    //for Android and Editor
    public Func<bool> IsHorizontal;
    public Func<bool> IsVertical;

    public Func<float> HorizontalControls;
    public Func<float> VerticalControls;



    public CharState state
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }


    void Awake()
    {
        IsHorizontal = () => fixedJoystick.Horizontal != 0;
        IsVertical = () => fixedJoystick.Vertical!= 0;

        HorizontalControls = () => fixedJoystick.Horizontal;
        VerticalControls = () => fixedJoystick.Vertical;

#if UNITY_EDITOR
        IsHorizontal = () => Input.GetButton("Horizontal");
        IsVertical = () => Input.GetButton("Vertical");

        HorizontalControls = () => Input.GetAxisRaw("Horizontal");
        VerticalControls = () => Input.GetAxisRaw("Vertical");
#endif


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        richtDirect = false;

    }


    void Start()
    {
        fixedJoystick.SnapX = true;
        fixedJoystick.SnapY = true;
        EventManager.Instance.AddListener(EVENT_TYPE.OpenShop, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CloseShop, this);
        EventManager.Instance.AddListener(EVENT_TYPE.FireButtonDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.FireButtonUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonLimit, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderWebHitCharacter, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderMeleeHitCharacter, this);
    }

    public void HorizontalFlip(Vector3 vector3)
    {
        if (vector3.x > 0 && !richtDirect) Flip();
        if (vector3.x < 0 && richtDirect) Flip();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetLadder();
        }

        if (IsHorizontal())
        {
            isLadder = CheckLadder();
            SetRigidbodyType2D(isLadder); 
            move = HorizontalControls();

            test = true;//for staying at ladder
            var directionHit = move > 0 ? Vector2.right : Vector2.left;
            GetRaycastHit(directionHit, horizontalRayRange);

            if (hit.collider != null)
            {
                canMove = false;
                if (hit.collider.name.Contains("block"))
                {
                    state = CharState.Rubilovo;
                }
            }
            else
            {
                canMove = true;
            }

            if (canMove && !blockMove)
            {
                Move(move, transform.right);
            }

            if (move > 0 && !richtDirect) Flip();
            if (move < 0 && richtDirect) Flip();
        }

        else if (IsVertical())
        {
            isLadder = CheckLadder();
            SetRigidbodyType2D(isLadder);

            move = VerticalControls();

            if (move < 0)
            {
                test = true; //for staying at ladder
            }

            var directionHit = move > 0 ? Vector2.up : Vector2.down;

            if (move > 0)
            {
                GetRaycastHit(directionHit, verticalRayRange);
            }
            else
            {
                GetRaycastHit(directionHit, verticalRayRange);
            }

            
            if (hit.collider?.name.Contains("block") == true)
            {
                state = CharState.Rubilovo;
                canMoveUp = false;
            }
            else if (hit.collider?.name.Contains("Stone") == true)
            {
                canMoveUp = false;
            }

            else if (hit.collider == null && isLadder)
            {
                canMoveUp = true;
            }

            else if (!isLadder)
            {
                canMoveUp = false;
                if (inBase)
                {
                    state = CharState.Idle;
                }
                else
                {
                    state = CharState.IdleKirche;
                }
            }

            if (canMoveUp && isLadder)
            {
                if (test == false && move > 0)
                {
                    //Idle
                }
                else
                {
                    Move(move, transform.up);
                }
            }
        }

        else
        {
            if (inBase)
            {
                state = CharState.Idle;
            }
            else
            {
                state = CharState.IdleKirche;
            }
            
        }

    }

    

    public void GetRaycastHit(Vector2 vector2, float rayRange)
    {

        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.45f), vector2 , rayRange, 1<<Layer.Blocks|1<<Layer.Stones);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), vector2 , Color.red, rayRange);

    }

    public void Move(float axisRaw, Vector3 vector3)
    {
        Vector2 vector2 = vector3 * axisRaw;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + vector2, speed * Time.deltaTime);

        if (inBase)
        {
            state = CharState.Walk;
        }
        else
        {
            state = CharState.WalkKirche;
        }
        
    }

    public void SetRigidbodyType2D(bool isLadder)
    {
        rb.bodyType = isLadder ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }

    public bool CheckLadder()
    {
        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.00f), 1<<Layer.Ladders );

        if (check != null && check.name.Contains("Ladder"))
        {
            Collider2D check2 = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.03f), 1 << Layer.Ladders);
            if (check2 == null)//for staying at ladder
            {
                if (inBase)
                {
                    state = CharState.Idle;
                }
                else
                {
                    state = CharState.IdleKirche;
                }
                test = false;
            }

            temp = rb.position.y;
                //Debug.Log(temp);

            return true;
        }

        //Debug.Log(temp);
        return false;
    }

    public void SetLadder()
    {
        //float posX = Mathf.Round(transform.position.x)+0.5f;
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
            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders |1<< Layer.Stones).collider != null)//стреляем вниз
            {
                Instantiate(ladder, pos, Quaternion.identity, sceneContainer); //если ничего не нашли внизу не пусто, то ставим лестницу
                test = true;
            }
        }
        else
        {
            //Debug.Log("Место занято, не могу поставить лестницу! Стоит: " + InstHit.collider.tag);
        }
    }

    public void HitBlock()
    {
        GameObject currentblock = hit.collider?.gameObject;
        if (currentblock != null)
        {
            BlockGroundDefault blockGroundDefault = currentblock.GetComponent<BlockGroundDefault>();
            blockGroundDefault?.Hit();
            //--
            //if (blockGroundDefault != null)
            //{
            //    blockGroundDefault.Notify += message => { count++; };
            //}
            //count1 = count / 3;
            //text.text = count1.ToString();
            //--
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
        Gizmos.DrawSphere(new Vector2(transform.position.x, transform.position.y + 0.03f), .025f);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        switch (Event_Type)
        {
            case EVENT_TYPE.OpenShop:
                blockMove = true;
                break;

            case EVENT_TYPE.CloseShop:
                blockMove = false;
                break;

            case EVENT_TYPE.FireButtonUp:
                if (GameStates.Instance.inTrebuchetPlace)
                {

                    animator.SetBool("TrebuchetCharge", false);
                }

                break;
            case EVENT_TYPE.FireButtonDown:
                if (GameStates.Instance.inTrebuchetPlace)
                {
                    animator.SetBool("TrebuchetCharge", true);
                    HorizontalFlip(transform.right);
                }

                break;

            case EVENT_TYPE.TrebSpoonDownPointerDown:
                if (GameStates.Instance.inTrebuchetPlace)
                {
                    animator.SetBool("TrebuchetSpoonDown", true);
                    HorizontalFlip(-transform.right);
                }

                break;

            case EVENT_TYPE.TrebSpoonDownPointerUp:
                if (GameStates.Instance.inTrebuchetPlace)
                {
                    animator.SetBool("TrebuchetSpoonDown", false);
                    HorizontalFlip(-transform.right);
                }

                animator.SetFloat("TrebSpoonSpeed", 1f);

                break;

            case EVENT_TYPE.TrebSpoonUpPointerDown:
                if (GameStates.Instance.inTrebuchetPlace)
                {
                    animator.SetBool("TrebuchetSpoonUp", true);
                    HorizontalFlip(-transform.right);
                }

                break;

            case EVENT_TYPE.TrebSpoonUpPointerUp:
                if (GameStates.Instance.inTrebuchetPlace)
                {
                    animator.SetBool("TrebuchetSpoonUp", false);
                    HorizontalFlip(-transform.right);
                }
                animator.SetFloat("TrebSpoonSpeed", 1f);

                break;

            case EVENT_TYPE.TrebSpoonLimit:
                animator.SetFloat("TrebSpoonSpeed", 0f);

                break;

            case EVENT_TYPE.SpiderWebHitCharacter:
                animator.Play("webDie");
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver,this);
                canMove = false;
                canMoveUp = false;
                break;

            case EVENT_TYPE.SpiderMeleeHitCharacter:
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver, this);
                animator.Play("die");
                break;


        }
    }
}

//когда стреляет может ходить


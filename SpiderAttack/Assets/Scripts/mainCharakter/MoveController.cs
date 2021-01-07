using System;
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
    private const string State = "State";
    private const string Block = "block";
    private const string Stone = "Stone";
    private const string Ladder = "Ladder";
        
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

    public bool isLadderAbove;
    private bool isLadder;
    public bool blockAllMoves;

    public FixedJoystick fixedJoystick;

    Animator animator;
    public bool blockMove = false;
    public bool canMove;
    public bool canMoveUp; //есть ли сверху препятствие
    //public bool inBase = true;

    //for Android and Editor
    public Func<bool> IsHorizontal;
    public Func<bool> IsVertical;

    public Func<float> VerticalControls;
    public Func<float> HorizontalControls;
    private Collider2D checkLadder;
    private Collider2D checkLadder2;
    public GameObject flashlight;

    public float testLadderCheck2 = 0.05f;



    public CharState state
    {
        get { return (CharState)animator.GetInteger(State); }
        set { animator.SetInteger(State, (int)value); }
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
        EventManager.Instance.AddListener(EVENT_TYPE.TrebFireButtonDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebFireButtonUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaFireButtonDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaFireButtonUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonLimit, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebCharge, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSetup, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderWebHitCharacter, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderMeleeHitCharacter, this);
        EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CharInCity, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaCharge, this);
        EventManager.Instance.AddListener(EVENT_TYPE.Teleport, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SetLadder, this);
    }

    public void HorizontalFlip(Vector3 vector3)
    {
        if (vector3.x > 0 && !richtDirect) Flip();
        if (vector3.x < 0 && richtDirect) Flip();
    }

    void Update()
    {

        if (IsHorizontal())
        {
            if (blockAllMoves)
            {
                return;
            }
            isLadder = CheckLadder();
            SetRigidbodyType2D(isLadder); 
            move = HorizontalControls();

            isLadderAbove = true;//for staying at ladder
            var directionHit = move > 0 ? Vector2.right : Vector2.left;
            GetRaycastHit(directionHit, horizontalRayRange);

            if (hit.collider != null)
            {
                canMove = false;
                if (hit.collider.name.Contains(Block))
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
            if (blockAllMoves)
            {
                return;
            }

            isLadder = CheckLadder();
            SetRigidbodyType2D(isLadder);

            move = VerticalControls();

            if (move < 0)
            {
                isLadderAbove = true; //for staying at ladder
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

            
            if (hit.collider?.name.Contains(Block) == true)
            {
                state = CharState.Rubilovo;
                canMoveUp = false;
            }
            else if (hit.collider?.name.Contains(Stone) == true)
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
                if (GameStates.Instance.InCity)
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
                if (isLadderAbove == false && move > 0)
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
            if (GameStates.Instance.InCity)
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
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), vector2 , Color.red, rayRange);

    }

    public void Move(float axisRaw, Vector3 vector3)
    {
        Vector2 vector2 = vector3 * axisRaw;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + vector2, speed * Time.deltaTime);

        if (GameStates.Instance.InCity)
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
        checkLadder = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.00f), 1<<Layer.Ladders| 1 << Layer.Static );


        //if (checkLadder != null && checkLadder.name.Contains(Ladder))
        if (checkLadder != null && checkLadder.CompareTag("stairs"))
        {
            //{
            checkLadder2 = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.03f), 1 << Layer.Ladders);
            if (checkLadder2 == null)//for staying at ladder
            {
                if (GameStates.Instance.InCity)
                {
                    state = CharState.Idle;
                }
                else
                {
                    state = CharState.IdleKirche;
                }
                isLadderAbove = false;
            }
            return true;
        }

        //Debug.Log(temp);
        return false;
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
            case EVENT_TYPE.BallistaCharge:
                animator.SetBool("WeaponCharge", true);
                HorizontalFlip(transform.right);

                break;

            case EVENT_TYPE.BallistaFireButtonUp:
                animator.SetBool("WeaponCharge", false);

                break;
            case EVENT_TYPE.TrebCharge:
                animator.SetBool("WeaponCharge", true);
                HorizontalFlip(transform.right);
                break;

            case EVENT_TYPE.TrebFireButtonUp:
                animator.SetBool("WeaponCharge", false);
                state = CharState.Idle;
                break;

            case EVENT_TYPE.BallistaShot:
                state = CharState.Idle;
                HorizontalFlip(transform.right);
                break;

            case EVENT_TYPE.TrebSetup:
                if (Param == null)
                {
                    break;
                }
                if ((int)Param == 1)
                {
                    animator.SetBool("WeaponMoveDown", true);
                    HorizontalFlip(-transform.right);
                }
                break;


            case EVENT_TYPE.TrebSpoonDownPointerDown:
                if (GameStates.Instance.inTrebuchetPlace || GameStates.Instance.inBallistaPlace)
                {
                    //animator.SetFloat("WeaponMoveSpeed", 1f);
                    animator.SetBool("WeaponMoveDown", true);
                    HorizontalFlip(-transform.right);
                }

                break;

            case EVENT_TYPE.TrebSpoonDownPointerUp:
                if (GameStates.Instance.inTrebuchetPlace || GameStates.Instance.inBallistaPlace)
                {
                    
                    animator.SetBool("WeaponMoveDown", false);
                    HorizontalFlip(-transform.right);
                }

                animator.SetFloat("WeaponMoveSpeed", 1f);

                break;

            case EVENT_TYPE.TrebSpoonUpPointerDown:
                if (GameStates.Instance.inTrebuchetPlace || GameStates.Instance.inBallistaPlace)
                {
                    //animator.SetFloat("WeaponMoveSpeed", 1f);
                    animator.SetBool("WeaponMoveUp", true);
                    HorizontalFlip(-transform.right);
                }

                break;

            case EVENT_TYPE.TrebSpoonUpPointerUp:
                if (GameStates.Instance.inTrebuchetPlace || GameStates.Instance.inBallistaPlace)
                {
                    
                    animator.SetBool("WeaponMoveUp", false);
                    HorizontalFlip(-transform.right);
                }
                animator.SetFloat("WeaponMoveSpeed", 1f);//off limit

                break;

            case EVENT_TYPE.TrebSpoonLimit:
                animator.SetFloat("WeaponMoveSpeed", 0f);
                //animator.SetBool("WeaponMoveDown", false);
                //animator.SetBool("WeaponMoveUp", false);

                break;

            case EVENT_TYPE.SpiderWebHitCharacter:
                animator.Play("webDie");
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver,this);
                blockAllMoves = true;
                break;

            case EVENT_TYPE.SpiderMeleeHitCharacter:
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver, this);
                animator.Play("die");
                break;

            case EVENT_TYPE.SetLadder:
                isLadderAbove = true;
                break;

            case EVENT_TYPE.GameOver:
                blockAllMoves = true;
                break;
                case EVENT_TYPE.Teleport: 
                    GameStates.Instance.InCity = false; 
                    state = CharState.IdleKirche;
                break;

            case EVENT_TYPE.CharInCity:
                if (Param != null)
                {
                    var inCity = (bool)Param;
                    if (inCity)
                    {
                        flashlight.SetActive(false);
                    }
                    else
                    {
                        flashlight.SetActive(true);
                    }
                    
                }
                break;
        }
    }
}

//когда стреляет может ходить
//when release the button camera jumps


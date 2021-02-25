using System;
using System.Collections;
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
    private const string Block = "block";
    //private const string Stone = "Stone";  
    public int count;
    private Rigidbody2D rb;
    private float move;
    public bool richtDirect;
    public float speed = 2f;
    RaycastHit2D hit;
    private float horizontalRayRange = 0.4f;
    private float verticalRayRange = 0.6f;
    public bool isLadderAbove = true;
    private bool isLadder;
    public bool blockAllMoves;
    public FixedJoystick fixedJoystick;
    Animator animator;
    public bool blockMove;
    public bool canMove;
    public bool canMoveUp;
    private IHitableBlock _blockGroundDefault;
    [SerializeField]
    private Transform _charCanvas;
    private Vector3 _charCanvasScale = new Vector3(0.01f, 0.01f, 1);


    //for Android and Editor
    public Func<bool> IsHorizontal;
    public Func<bool> IsVertical;

    public Func<float> VerticalControls;
    public Func<float> HorizontalControls;
    private Collider2D checkLadder;
    private Collider2D checkLadder2;
    public GameObject flashlight;
    private bool _inMoveToTarget;


    public CharState State
    {
        get => (CharState)animator.GetInteger("State"); 
        set => animator.SetInteger("State", (int)value); 
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
        EventManager.Instance.AddListener(EVENT_TYPE.CheckIfCharInVillage, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaCharge, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartTeleport, this);
        EventManager.Instance.AddListener(EVENT_TYPE.FinishTeleport, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SetLadder, this);
        EventManager.Instance.AddListener(EVENT_TYPE.DynamiteHurtChar, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CharMoveToTarget, this);

        CheckIfInVillage();
        SetRigidbodyType2D(CheckLadder());
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

            //GetRaycastHitAllLayers(Vector2.down, horizontalRayRange);
            //if (hit.collider == null) return;

            isLadderAbove = true;//for staying at ladder
            var directionHit = move > 0 ? Vector2.right : Vector2.left;
            GetRaycastHit(directionHit, horizontalRayRange);

            if (hit.collider != null)
            {
                canMove = false;
                if (hit.collider.name.Contains(Block))
                {
                    TryHitBlock();
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
                TryHitBlock();
                canMoveUp = false;
            }

            else if (move > 0 && hit.collider?.CompareTag("staticBlock") == true)
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
                SetIdleAnimation();
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
            SetIdleAnimation();
        }
    }

    public bool CheckIfInVillage()
    {
        var pos = transform.position;
        if (-4f < pos.x && pos.x < 30)
        {
            if (-0.5f < transform.position.y && transform.position.y < 10)
            {
                GameStates.Instance.InCity = true;
                return true;
            }
        }
        GameStates.Instance.InCity = false;
        return false;
    }

    public bool CheckIfNearByCave()
    {
        var pos = transform.position;
        if (-15.5f < pos.x && pos.x < 30)
        {
            if (-0.5f < transform.position.y && transform.position.y < 10)
            {
                return true;
            }
        }
        return false;
    }

    public void GetRaycastHit(Vector2 vector2, float rayRange)
    {

        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.45f), vector2 , rayRange, 1<<Layer.Blocks | 1<<Layer.Stones | 1 << Layer.Static | 1 << Layer.GateRight );
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), vector2 , Color.red, rayRange);

    }

    //public void GetRaycastHitAllLayers(Vector2 vector2, float rayRange)
    //{
    //    hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.45f), vector2, rayRange, ~( 1 << 20));
    //}

    public void Move(float axisRaw, Vector3 vector3)
    {
        Vector2 vector2 = vector3 * axisRaw;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + vector2, speed * Time.deltaTime);

        SetWalkAnimation();
    }


    private IEnumerator MoveToTarget(Transform objTransform, Vector3 from, Vector3 to)
    {
        _inMoveToTarget = true;
        blockAllMoves = true;
        var t = 0f;
        HorizontalFlip(to - objTransform.position);
        
        while (t < 1f)
        {
            SetWalkAnimation();
            t += Time.deltaTime;
            objTransform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        _inMoveToTarget = false;
        blockAllMoves = false;
    }

    private void SetWalkAnimation()
    {
        if (GameStates.Instance.InCity )
        {
            if (State != CharState.Walk)
            {
                State = CharState.Walk;
            }
        }
        if (!GameStates.Instance.InCity)
        {
            if (State != CharState.WalkKirche)
            {
                State = CharState.WalkKirche;
            }
        } 
    }

    private void SetIdleAnimation()
    {
        if (GameStates.Instance.InCity)
        {
            if (State != CharState.Idle)
            {
                State = CharState.Idle;
            }
        }
        if (!GameStates.Instance.InCity)
        {
            if (State != CharState.IdleKirche)
            {
                State = CharState.IdleKirche;
            }
        }
    }

    public void SetRigidbodyType2D(bool isLadder)
    {
        rb.bodyType = isLadder ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }

    public bool CheckLadder()
    {
        checkLadder = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.00f), 1<<Layer.Ladders| 1 << Layer.Static | 1 << Layer.Teleports);

        if (checkLadder != null && checkLadder.CompareTag("stairs"))
        {
            checkLadder2 = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.03f), 1 << Layer.Ladders | 1 << Layer.Teleports);
            if (checkLadder2 == null)//for staying at ladder
            {
                SetIdleAnimation();
                isLadderAbove = false;
            }
            return true;
        }

        return false;
    }

    private void TryHitBlock()
    {
        _blockGroundDefault = hit.collider?.gameObject.GetComponent<IHitableBlock>();
        if (_blockGroundDefault == null) return;

        if (_blockGroundDefault.MinPickLvl > GameStates.PickLvl)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.NeedNextPick, this, _blockGroundDefault.MinPickLvl);
            State = CharState.IdleKirche;
            return;
        }

        if (!_blockGroundDefault.IsGround && GameStates.BackpackCurrentQty >= GameStates.BackpackCapacity)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.NeedNextBackpack, this,
                _blockGroundDefault.MinPickLvl);
            State = CharState.IdleKirche;
            return;
        }

        State = CharState.Rubilovo;
    }

    public void HitBlock()
    {
        _blockGroundDefault?.Hit();
    }

    void Flip()
    {
        richtDirect = !richtDirect;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        FlipCharCanvas(scale);
    }

    private void FlipCharCanvas(Vector3 scale)
    {
        if (scale.x < 0)
        {
            var charCanvasScale = _charCanvasScale;
            charCanvasScale.x = - 0.01f;
            _charCanvas.localScale = charCanvasScale;
        }
        else
        {
            _charCanvas.localScale = _charCanvasScale;
        }
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
                blockAllMoves = true;
                break;

            case EVENT_TYPE.BallistaFireButtonUp:
                animator.SetBool("WeaponCharge", false);
                if (!_inMoveToTarget)
                {
                    blockAllMoves = false;
                }

                break;

            case EVENT_TYPE.TrebCharge:
                animator.SetBool("WeaponCharge", true);
                HorizontalFlip(transform.right);
                blockAllMoves = true;
                break;

            case EVENT_TYPE.TrebFireButtonUp:
                animator.SetBool("WeaponCharge", false);
                State = CharState.Idle;
                if (!_inMoveToTarget)
                {
                    blockAllMoves = false;
                }
                break;

            case EVENT_TYPE.BallistaShot:
                State = CharState.Idle;
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

                break;

            case EVENT_TYPE.SpiderWebHitCharacter:
                animator.SetBool("WebDie", true);
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver,this);
                blockAllMoves = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
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

             case EVENT_TYPE.StartTeleport:
                animator.SetBool("Teleport", true);
                blockMove = true;
                break;

            case EVENT_TYPE.FinishTeleport:
                animator.SetBool("Teleport", false);
                EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);
                blockMove = false;
                SetRigidbodyType2D(CheckLadder());
                break;

            case EVENT_TYPE.CheckIfCharInVillage:
                {
                    if (CheckIfInVillage())
                    {
                        State = CharState.Idle;
                        break;
                    }
                    else
                    {
                        State = CharState.IdleKirche;
                        break;
                    }
                }

            case EVENT_TYPE.DynamiteHurtChar:
            {
                animator.SetBool("Die", true);
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver, this, true);
                    break;
            }

            case EVENT_TYPE.CharMoveToTarget:
            {
                if (Param == null) return;
                var target = (Vector3)Param;
                if (_inMoveToTarget == false)
                {
                    StartCoroutine(MoveToTarget(gameObject.transform, gameObject.transform.position, target));
                }
               

                break;
            }

        }
    }
}
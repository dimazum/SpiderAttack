using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;
using MarchingBytes;
using UnityEngine;
using UnityEngine.UI;


public class Ballista : MonoBehaviour, IListener
{
    //private Animator _animator;
    public Animation _animation;
    public GameObject arrow;
    public Transform arrowStart;
    public float _BowBasePower = 200;
    public float rotatePower = -0.01f;
    public float _NormTime;
    private string _currentAnim;
    private bool _isCharging;
    private bool _inBallistaPlace;

    public Image sliderBallista;
    public bool _rotateSmallGearUp;
    public bool _rotateSmallGearDown;
    public Transform BowBase;
    public Transform SmallGear;
    public float _BowBaseRotateSpeed = 10;
    public float _smallGearSpeed = 100;
    private UIController _uiController;
    public ItemCategory itemCategory;

    private float _counter;
    public float Counter
    {
        get => _counter;
        set
        {
            _counter = value;
            if (_counter < 0)
            {
                _counter = 0;
            }
        }
    }


    void Awake()
    {
        //_animator = GetComponent<Animator>();
    }

    void Start()
    {
        _uiController = FindObjectOfType<UIController>();
        sliderBallista.fillAmount = 0;
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaFireButtonUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaFireButtonDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerUp, this);

    }

    void FixedUpdate()
    {
        if (_isCharging && Counter < 100)
        {
            Counter += 2;
            //sliderBallista.fillAmount = i / 100;
            
        }
        if (!_isCharging && Counter > 0)
        {
            Counter -= 1.5f;
        }
        _uiController.ballistaSliderCharge.fillAmount = Counter / 100;
    }

    void Update()
    {
        if (_inBallistaPlace)
        {
            RotateSmallGearUp();
            RotateSmallGearDown();
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("player"))
        {
            GameStates.Instance.inBallistaPlace = true;
            _inBallistaPlace = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("player"))
        {
            GameStates.Instance.inBallistaPlace = false;
            _inBallistaPlace = false;
        }
    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.BallistaFireButtonUp:

                if (!GameStates.Instance.inBallistaPlace)
                {
                    break;
                }

                if (Counter > 99)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.BallistaIsCharged, this);
                    break;
                }

                var time = CorrectTime(_animation["BallistaCharge"].normalizedTime);
                SetAnimation("BallistaCharge", -1f, time);

                Counter = 0;
                _isCharging = false;
                sliderBallista.fillAmount = 0;
                break;


            case EVENT_TYPE.BallistaFireButtonDown:
                if (!GameStates.Instance.inBallistaPlace)
                {
                    //SetAnimation("BallistaPlaceHighlight");
                    break;
                }

                if (Counter > 99)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.BallistaShot, this, itemCategory);
                    SetAnimation("BallistaShot");
                    _isCharging = false;
                    Shot();
                    Counter = 0;
                    sliderBallista.fillAmount = 0;

                    break;
                }

                EventManager.Instance.PostNotification(EVENT_TYPE.BallistaCharge, this);
                SetAnimation("BallistaCharge", 1);
                _isCharging = true;

                break;

            case EVENT_TYPE.TrebSpoonDownPointerDown:

                if (!GameStates.Instance.inBallistaPlace)
                {
                    //SetAnimation("BallistaPlaceHighlight");
                    break;
                }

                _rotateSmallGearDown = true;

                break;

            case EVENT_TYPE.TrebSpoonDownPointerUp:
                _rotateSmallGearDown = false;

                break;

            case EVENT_TYPE.TrebSpoonUpPointerDown:

                if (!GameStates.Instance.inBallistaPlace)
                {
                    //SetAnimation("BallistaPlaceHighlight");
                    break;
                }
                _rotateSmallGearUp = true;
                break;

            case EVENT_TYPE.TrebSpoonUpPointerUp:
                _rotateSmallGearUp = false;

                break;
        }
    }


    public void Shot()
    {
        //GameObject arrow_clone = Instantiate(arrow, arrowStart.position, arrowStart.rotation, arrowStart);
        //arrow_clone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * _BowBasePower);
        //arrow_clone.GetComponent<Rigidbody2D>().AddTorque(rotatePower, ForceMode2D.Force);

        //Destroy(arrow_clone, 3f);


        //var arr = leanGameObjectPool.Spawn(arrowStart.position, arrowStart.rotation, arrowStart);
        //arr?.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * _BowBasePower);


        GameObject go = EasyObjectPool.instance.GetObjectFromPool(itemCategory.ToString(), arrowStart.position, arrowStart.rotation);
        //go?.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * _BowBasePower);

    }

    //public IEnumerator DespawnDelay()
    //{
    //    yield return new WaitForSeconds(2);

    //}

    private float CorrectTime(float time)
    {
        if (time < 0)
        {
            time = 0;
        }
        else if (time > 1)
        {
            time = .99f;
        }

        return time;
    }

    public void SetAnimation(string nameAnim, float speed = 1, float normTime = 0)
    {
        _animation.Play(nameAnim);
        _currentAnim = nameAnim;
        _animation[nameAnim].speed = speed;
        _animation[nameAnim].normalizedTime = normTime;
    }

    public void RotateSmallGearDown()
    {
        if (_rotateSmallGearDown)
        {

            var pos = BowBase.transform.localEulerAngles.z;

            if (pos < 11 || pos > 350)
            {
                BowBase.transform.Rotate(-(Vector3.forward) * _BowBaseRotateSpeed * Time.deltaTime);
                SmallGear.transform.Rotate(-(Vector3.forward) * _smallGearSpeed * Time.deltaTime);
            }
            else
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonLimit, this);
            }
        }
    }

    public void RotateSmallGearUp()
    {
        if (_rotateSmallGearUp)
        {
            var pos = BowBase.transform.localEulerAngles.z;
            if (pos < 10 || pos > 349)
            {
                BowBase.transform.Rotate((Vector3.forward) * _BowBaseRotateSpeed * Time.deltaTime);
                SmallGear.transform.Rotate((Vector3.forward) * _smallGearSpeed * Time.deltaTime);
            }
            else
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonLimit, this);
            }
        }

    }

}

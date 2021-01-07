
using Assets.Scripts.UI;
using MarchingBytes;
using UnityEngine;


namespace Assets.Scripts.Weapons
{
    public class Trebuchet : MonoBehaviour, IListener, IWeapon
    {
        public Transform SpoonAim;
        public Transform Spoon;
        public Transform SmallGear;
        public Transform rod;
        public Transform mockGear;
        public GameObject topBelt;
        public GameObject bottomBelt;
        private Animation _animation;
        private float _mockGearSpeed = 70f;
        private float _rodSpeed = .03f;
        private float _spoonSpeed = 10;
        private float _smallGearSpeed = 70;
        private float _sila = 500;
        private UIController _uiController;
        private float _i;
        private EasyObjectPool _easyObjectPool;
        public int chargeDuration = 100;
        public bool _isCharging;
        public bool _isShot;
        public bool _rotateSmallGearUp;
        public bool _rotateSmallGearDown;
        private bool _inTrebuchetPlace;

        public float I
        {
            get => _i;
            set
            {
                _i = value;
                if (_i < 0)
                {
                    _i = 0;
                }
            } 
        }

        public ItemCategory itemCategory { get; set; }

        
        void Start()
        {
            _animation = GetComponent<Animation>();
            itemCategory = ItemCategory.BallX1;
            _easyObjectPool = GetComponent<EasyObjectPool>();
            _uiController = FindObjectOfType<UIController>();
            EventManager.Instance.AddListener(EVENT_TYPE.TrebFireButtonUp, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebFireButtonDown, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerDown, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerUp, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerDown, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerUp, this);

        }
        void FixedUpdate()
        {
            if (_isCharging && I < chargeDuration)
            {
                I += 2;
            }
            if (!_isCharging && I > 0)
            {
                I -= 1.5f;
            }

            _uiController.trebSliderCharge.fillAmount = I / chargeDuration;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                InvokeRepeating("Shot", 1f, 2f);
            }

            if (_inTrebuchetPlace)
            {
                RotateSmallGearUp();
                RotateSmallGearDown();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == ("player"))
            {
                GameStates.Instance.inTrebuchetPlace = true;
                _inTrebuchetPlace = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == ("player"))
            {
                GameStates.Instance.inTrebuchetPlace = false;
                _inTrebuchetPlace = false;
            }
        }


        public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case EVENT_TYPE.TrebFireButtonDown:

                    if (GameStates.Instance.inBallistaPlace)
                    {
                        break;
                    }

                    if (!GameStates.Instance.inTrebuchetPlace)
                    {
                        //SetAnimation("TrebPlaceHighlight");
                        break;
                    }

                    if (I > chargeDuration - 1)
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.BallistaShot, this);
                        _isCharging = false;
                        _isShot = true;
                        SetAnimation("TrebShot");
                        break;
                    }

                    if (I == 0)
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.TrebCharge, this);
                        SetAnimation("TrebCharge", 1);
                        _isCharging = true;
                        _isShot = false;
                    }
                    break;

                case EVENT_TYPE.TrebFireButtonUp:

                    if (!GameStates.Instance.inTrebuchetPlace)
                    {
                        break;
                    }
                    if (I > chargeDuration - 1)
                    {
                        break;
                    }

                    if (_isShot)
                    {
                        break;
                    }

                    var time = CorrectTime(_animation["TrebCharge"].normalizedTime);
                    SetAnimation("TrebCharge", -1f, time);

                    _isCharging = false;

                    break;

                case EVENT_TYPE.TrebSpoonUpPointerDown:


                    if (!GameStates.Instance.inTrebuchetPlace)
                    {
                        //SetAnimation("TrebPlaceHighlight");
                        break;
                    }

                    _rotateSmallGearUp = true;

                    break;

                case EVENT_TYPE.TrebSpoonUpPointerUp:

                    _rotateSmallGearUp= false;
                    break;

                case EVENT_TYPE.TrebSpoonDownPointerDown:

                    if (!GameStates.Instance.inTrebuchetPlace)
                    {
                        //SetAnimation("TrebPlaceHighlight");
                        break;
                    }
                    _rotateSmallGearDown = true;

                    break;

                case EVENT_TYPE.TrebSpoonDownPointerUp:

                    _rotateSmallGearDown = false;

                    break;
            }
        }

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
            _animation[nameAnim].speed = speed;
            _animation[nameAnim].normalizedTime = normTime;
        }


        public void Shot()
        {

            GameObject bulletClone = _easyObjectPool.GetObjectFromPool(itemCategory.ToString(), SpoonAim.position, SpoonAim.rotation);
            bulletClone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, 1, 0) * _sila);
            EventManager.Instance.PostNotification(EVENT_TYPE.TrebShot, this);

        }

        public void RotateSmallGearUp()
        {
            if (_rotateSmallGearUp)
            {

                var pos = Spoon.transform.localEulerAngles.z;

                if (pos < 11 || pos > 350)
                {
                    Spoon.transform.Rotate(-(Vector3.forward) * _spoonSpeed * Time.deltaTime);
                    SpoonAim.transform.Rotate(-(Vector3.forward) * _spoonSpeed * Time.deltaTime);
                    SmallGear.transform.Rotate(-(Vector3.forward) * _smallGearSpeed * Time.deltaTime);
                    rod.Translate(-Vector3.right * _rodSpeed * Time.deltaTime);
                    mockGear.Rotate(-(Vector3.forward) * _mockGearSpeed * Time.deltaTime);
                    topBelt.GetComponent<SpriteRenderer>().size = new Vector2(topBelt.GetComponent<SpriteRenderer>().size.x+(-Time.deltaTime), .2f);
                    bottomBelt.GetComponent<SpriteRenderer>().size = new Vector2(bottomBelt.GetComponent<SpriteRenderer>().size.x + (Time.deltaTime), .2f);
                }
                else
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonLimit, this);
                }
            }
        }

        public void RotateSmallGearDown()
        {
            if (_rotateSmallGearDown)
            {
                var pos = Spoon.transform.localEulerAngles.z;
                if (pos < 10 || pos > 349)
                {
                    Spoon.transform.Rotate(Vector3.forward * _spoonSpeed * Time.deltaTime);
                    SpoonAim.transform.Rotate(Vector3.forward * _spoonSpeed * Time.deltaTime);
                    SmallGear.transform.Rotate(Vector3.forward * _smallGearSpeed * Time.deltaTime);
                    rod.Translate(Vector3.right * _rodSpeed * Time.deltaTime);
                    mockGear.transform.Rotate(Vector3.forward * _mockGearSpeed * Time.deltaTime);
                    topBelt.GetComponent<SpriteRenderer>().size = new Vector2(topBelt.GetComponent<SpriteRenderer>().size.x + (Time.deltaTime), .2f);
                    bottomBelt.GetComponent<SpriteRenderer>().size = new Vector2(bottomBelt.GetComponent<SpriteRenderer>().size.x + (-Time.deltaTime), .2f);
                }
                else
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonLimit, this);
                }
            }

        }
    }
}

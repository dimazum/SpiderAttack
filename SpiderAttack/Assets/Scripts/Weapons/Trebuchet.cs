using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapons
{
    public class Trebuchet : MonoBehaviour, IListener
    {
        private Animator _animator;
        private bool inFirePlace;
        public GameObject Spoon;
        public GameObject SmallGear;
        public GameObject BigGear;
        public GameObject BigGearAim;
        public GameObject Bullet;

        public Transform SpoonAim;
        private float _smallGearSpeed = 500f;
        private float _bigGearSpeed = 200f;
        private float _spoonSpeed = 10f;
        private float _sila = 500;

        private bool _rotateSmallGearDown;
        private bool _rotateSmallGearUp;
        private bool _rotateBigGearCharge;
        private bool _rotateBigGearDischarge;
        public bool _isCharged;

        private int _bigGearCounter;

        public float i = 0;

        void Awake()
        { 
            _animator = GetComponent<Animator>();
        }
        
        void Start()
        {
            EventManager.Instance.AddListener(EVENT_TYPE.FireButtonUp, this);
            EventManager.Instance.AddListener(EVENT_TYPE.FireButtonDown, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerDown, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerUp, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerDown, this);
            EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerUp, this);
        }
        void FixedUpdate()
        {
       
        }

        void Update()
        {
            RotateSmallGearDown();
            RotateSmallGearUp();
            BigGearCharge();
            BigGearDisCharge();

            if (Input.GetKeyDown(KeyCode.C))
            {
                InvokeRepeating("ShotTest", 1f, 2f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == ("player"))
            {
                inFirePlace = true;
                GameStates.Instance.inTrebuchetPlace = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == ("player"))
            {
                inFirePlace = false;
                GameStates.Instance.inTrebuchetPlace = false;
            }
        }


        public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case EVENT_TYPE.FireButtonUp:

                    if (!GameStates.Instance.inTrebuchetPlace)
                    {
                        _animator.Play("PlaceHighlight");
                        return;
                    }

                    if (_isCharged)
                    {
                        _animator.Play("TrebuchetShot");
                        _isCharged = false;
                        
                    }
                    else
                    {
                        _rotateBigGearCharge = false;
                        _rotateBigGearDischarge = true;
                    }
                    
                    break;


                case EVENT_TYPE.FireButtonDown:                                                                                                                                                                                                                                                                                                                    
                    if (!GameStates.Instance.inTrebuchetPlace)
                    {
                        _animator.Play("PlaceHighlight");
                        return;
                    }
                    
                    _rotateBigGearDischarge = false;
                    _rotateBigGearCharge = true;
                    
                    break;

                case EVENT_TYPE.TrebSpoonUpPointerDown:
                    _rotateSmallGearUp = true;
                    break;

                case EVENT_TYPE.TrebSpoonUpPointerUp:
                    _rotateSmallGearUp = false;
                    break;

                case EVENT_TYPE.TrebSpoonDownPointerDown:
                    _rotateSmallGearDown = true;
                    break;

                case EVENT_TYPE.TrebSpoonDownPointerUp:
                    _rotateSmallGearDown = false;
                    break;
            }
        }

        void BigGearCharge()
        {
            if (_rotateBigGearCharge)
            {
                if (!inFirePlace)
                {
                    _animator.Play("PlaceHighlight");
                    return;
                }
                var pos =  BigGear.transform.localEulerAngles.z ;

                if (pos > 0)
                {
                    pos -= 360;
                }

                if (pos == 0 || pos > -200)
                {
                    BigGear.transform.Rotate(-(Vector3.forward) * _bigGearSpeed * Time.deltaTime);
                }

                if (pos < -200)
                {
                    _isCharged = true;
                }
            }
        }

        void BigGearDisCharge()
        {
            if (_rotateBigGearDischarge)
            {
                var pos2 = BigGear.transform.localEulerAngles.z;

                if (pos2 <= 355)
                {
                    BigGear.transform.Rotate((Vector3.forward) * _bigGearSpeed * Time.deltaTime);
                }

                if (pos2 > 355)
                {
                    BigGear.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    _rotateBigGearDischarge = false;

                } 
            }
        }

        public void RotateSmallGearDown()
        {
            if (_rotateSmallGearDown)
            {
                if (!inFirePlace)
                {
                    _animator.Play("PlaceHighlight");
                    return;
                }

                var pos = Spoon.transform.localEulerAngles.z;

                if (pos < 11 || pos > 350)
                {
                    Spoon.transform.Rotate(-(Vector3.forward) * _spoonSpeed * Time.deltaTime);
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
                if (!inFirePlace)
                {
                    _animator.Play("PlaceHighlight");
                    return;
                }

                var pos = Spoon.transform.localEulerAngles.z;
                if(pos < 10 || pos > 349)
                {
                    Spoon.transform.Rotate((Vector3.forward) * _spoonSpeed * Time.deltaTime);
                    SmallGear.transform.Rotate((Vector3.forward) * _smallGearSpeed * Time.deltaTime);
                }
                else
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonLimit, this);
                }
            }
          
        }

        public void Shot()
        {
            _rotateBigGearCharge = false;
            _rotateBigGearDischarge = true;

            //Destroy(bulletStatic);

            GameObject bulletClone = Instantiate(Bullet, SpoonAim.position, SpoonAim.rotation);

            bulletClone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, 1, 0) * _sila);

            EventManager.Instance.PostNotification(EVENT_TYPE.TrebShot, this, bulletClone.transform);


            Destroy(bulletClone, 3f);


        }


        public void ShotTest()
        {

            GameObject bulletClone = Instantiate(Bullet, SpoonAim.position, SpoonAim.rotation);

                bulletClone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(Random.Range(.7f,.9f), 1, 0) * _sila);

                EventManager.Instance.PostNotification(EVENT_TYPE.TrebShot, this, bulletClone.transform);


                Destroy(bulletClone, 3f);

        }
    }
}

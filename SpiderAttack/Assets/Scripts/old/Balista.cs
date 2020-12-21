using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Balista : MonoBehaviour, IListener {
    public Transform pricel;
    public GameObject arrow;
    public float i = 0;
    public float force = 10f;
    public float Sila = 150;
    public float rotatePower = -20f ;

    public bool fire;
    public bool rotateRight;
    public bool rotateLeft;

    public bool isReadyToShoot = false;

    public GameObject geer;
    private bool _rotateSmallGearUp;
    private bool _rotateSmallGearDown;
    private Coroutine _shotCoroutine;

    // Use this for initialization
    void Start ()
    {

        EventManager.Instance.AddListener(EVENT_TYPE.TrebFireButtonDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebFireButtonUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonDownPointerUp, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerDown, this);
        EventManager.Instance.AddListener(EVENT_TYPE.TrebSpoonUpPointerUp, this);
    }


    void Update () {

        if (_rotateSmallGearDown){

            var pos = geer.transform.localEulerAngles.z;

            geer.transform.Rotate(-(Vector3.forward) * 6 * Time.deltaTime);

        }
        if (_rotateSmallGearUp)
        {

            var pos = geer.transform.localEulerAngles.z;
            geer.transform.Rotate(Vector3.forward * 6 * Time.deltaTime);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("player"))
        {
            GameStates.Instance.inBallistaPlace = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("player"))
        {
            GameStates.Instance.inBallistaPlace = false;
        }
    }
    public void RotateRightDown()
    {
        rotateRight = true;
    }
    public void RotateRightUp()
    {
        rotateRight = false;
    }
    public void RotateLeftDown()
    {
        rotateLeft = true;
    }
    public void RotateLeftUp()
    {
        rotateLeft = false;
    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.TrebFireButtonUp:
                if (GameStates.Instance.inBallistaPlace)
                {
                    i = 0;
                    //sliderBallista.value = 0;
                    //sliderBallista.fillAmount = 0;
                    if (_shotCoroutine != null)
                    {
                        StopCoroutine(_shotCoroutine);
                    }
                }
                
                if (isReadyToShoot)
                {
                    GameObject arrow_clone = Instantiate(arrow, pricel.position, pricel.rotation, pricel);
                    arrow_clone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * Sila);
                    arrow_clone.GetComponent<Rigidbody2D>().AddTorque(rotatePower, ForceMode2D.Force);
                    isReadyToShoot = false;
                }
                if (!GameStates.Instance.inTrebuchetPlace)
                {
                    //_animator.Play("PlaceHighlight");
                    return;
                }

                break;


            case EVENT_TYPE.TrebFireButtonDown:

                if (GameStates.Instance.inBallistaPlace)
                {
                    //_shotCoroutine = StartCoroutine(ShotCounter());
                }
                if (!GameStates.Instance.inTrebuchetPlace)
                {
                    //_animator.Play("PlaceHighlight");
                    return;
                }


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

    //public IEnumerator ShotCounter()
    //{
    //    while (i < 60)
    //    {
    //        sliderBallista.fillAmount += .02f;
    //        yield return new WaitForSeconds(.01f);
    //        i++;
    //    }
    //    isReadyToShoot = true;
    //}
}

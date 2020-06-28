using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balista : MonoBehaviour {
    public Transform povorot;
    public Transform pricel;
    public GameObject arrow;
    public float i = 0;
    public float force = 10f;
    public float Sila = 50000;

    public bool fire;
    public bool rotateRight;
    public bool rotateLeft;
    public GameObject StrelkaPovorota;

    public Slider SlideWeapon;
    // Use this for initialization
    void Start () {
        

    }
    void FixedUpdate()
    {
        if (fire)
        {
           
            if (i <= 99)
            {
                                i = i + 100 * Time.deltaTime;
            }
            //else i = 0;
        }
    }
    // Update is called once per frame
    void Update () {
        // SlideAttack.transform.localScale = new Vector3(i / 100f, 1,1);
        SlideWeapon.value = i / 100;
        if (rotateLeft)
        {
            povorot.transform.Rotate(Vector3.forward * force * Time.deltaTime);
            StrelkaPovorota.transform.Rotate(Vector3.forward * 50f * Time.deltaTime / 4f);
        }
        if (rotateRight)
        {
            povorot.transform.Rotate(-Vector3.forward * force * Time.deltaTime);
            StrelkaPovorota.transform.Rotate(-Vector3.forward * 50f* Time.deltaTime / 4f);
        }
        //if (fire)
        //{
        //    if (i >= 98)
        //    {
        //        GameObject arrow_clone = Instantiate(arrow, pricel.position, pricel.rotation);
        //        arrow_clone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * Sila);
        //    }
        //    i = 0;
        //}
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
    public void FireFixedUp()
    {
        fire = false;

        if (i >= 98)
        {
            if (i >= 98)
            {
                GameObject arrow_clone = Instantiate(arrow, pricel.position, pricel.rotation);
                arrow_clone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(1, 0, 0) * Sila);
            }
            i = 0;
        }
        i = 0;
    }
    public void FireFixedDown()
    {
        fire = true;


    }
}

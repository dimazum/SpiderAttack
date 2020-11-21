using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrebuchetOld : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletStatic;
    public GameObject logka;
    public GameObject begunok;
    public GameObject shtanga;
    
   
    public Transform pricel;
    public Transform povorot;
    public GameObject Rul;
    public Transform Rul2;
    public float force = 10f;
    public float speedRul = 50f;
    public float Sila = 50000;
    public float i = 0;
    public Animator animator;

    public bool fire;
    public bool rotateRight=false;
    public bool rotateLeft=false;

    public bool inTrebucheteTrigger;


    public Slider SlideWeapon;
    public GameObject StrelkaPovorota;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
     void FixedUpdate()
    {
       
        if (fire&& inTrebucheteTrigger)
        {
            animator.Play("zaryad");
            if (i <= 99)
            {

                i = i + 100 * Time.deltaTime;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("player"))
        {
            //Debug.Log("Zaselllll");
            inTrebucheteTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("player"))
        {
            inTrebucheteTrigger = false;
        }
    }
    void Update()
    {
        
        SlideWeapon.value = i / 100;
        

       
            if (rotateRight==true)
            {
            povorot.transform.Rotate(-(Vector3.forward) * force * Time.deltaTime);
            Rul.transform.Rotate(-(Vector3.forward) * speedRul * Time.deltaTime * 5f);
            //Rul2.Rotate(-(Vector3.forward) * speedRul * Time.deltaTime * 5f);//
            begunok.transform.Translate(-logka.transform.right * Time.deltaTime / 50f);
            StrelkaPovorota.transform.Rotate(-Vector3.forward * speedRul * Time.deltaTime / 4f);
            
        }
        if (rotateLeft==true)
        {
            povorot.transform.Rotate(Vector3.forward * force * Time.deltaTime);
            Rul.transform.Rotate(Vector3.forward * speedRul * Time.deltaTime * 5f);
           // Rul2.Rotate(Vector3.forward * speedRul * Time.deltaTime * 5f);
            begunok.transform.Translate(logka.transform.right * Time.deltaTime / 50f);
            StrelkaPovorota.transform.Rotate(Vector3.forward * speedRul * Time.deltaTime / 4f);
        }
        
    }
    public void FireFixedDown()
    {
        fire = true;

        
    }
    public void FireFixedUp()
    {
        fire = false;

        if (i >= 98)
        {
            Destroy(bulletStatic);
            animator.Play("shoot");
            GameObject bullet_clone = Instantiate(bullet, pricel.position, pricel.rotation);
            
            bullet_clone.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, 1, 0) * i / 100f * Sila);


            Destroy(bullet_clone, 3f);
        }
        else animator.Play("zaryadOff");
        i = 0;
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
}

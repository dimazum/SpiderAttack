using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapons;
using UnityEngine;

public class TriggerBattle2 : MonoBehaviour
{
    public cameraController cC;
    public bool inBattle2 = false;
    public TriggerBattle Tb;
    public Trebuchet Tr;
    public Balista Bl;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            inBattle2 = true;
            if (inBattle2 == true && Tb.inBattle == true)
            {
                
                Tr.GetComponent<Trebuchet>().enabled = false;
                Bl.GetComponent<Balista>().enabled = true;
                cC.animator.Play("CameraInBattle2");
            }
        }
        
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            inBattle2 = false;
            if (inBattle2 == false && Tb.inBattle == true)
            {
                Bl.GetComponent<Balista>().enabled = false;
                Tr.GetComponent<Trebuchet>().enabled = true;

                cC.animator.Play("CameraOutBattle2");
            }
        }
    }
}

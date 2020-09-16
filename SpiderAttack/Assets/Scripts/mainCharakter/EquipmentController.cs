using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;

public class EquipmentController : MonoBehaviour, IListener
{
    //private int pickLvl;
    //public int PickLvl
    //{
    //    get { return pickLvl; }
    //    set
    //    {
    //        if (pickLvl < SpriteData.Instance.kirches.Length - 1)
    //        {
    //            pickLvl = value;
    //        }
    //    }
    //} 

    public GameObject Kirche;

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.PickUp:
                
                //Kirche.GetComponent<TextureController>().DisplayedSprite = GameStates.Instance.PickLvl;
                Kirche.GetComponent<SpriteRenderer>().sprite =
                    GameStates.Instance.CurrentPick.GetComponent<Pick>().sprite;
                break;
        }
    }

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.PickUp, this);

        Kirche.GetComponent<SpriteRenderer>().sprite =
            GameStates.Instance.CurrentPick.GetComponent<Pick>().sprite;

        //Kirche.GetComponent<TextureController>().DisplayedSprite = kircheLvl;
    }


}

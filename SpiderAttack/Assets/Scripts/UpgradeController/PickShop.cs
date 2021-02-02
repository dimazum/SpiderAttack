﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//public class PickShop : MonoBehaviour, IListener
//{
//    public GameObject CurrentPick;
//    public GameObject NewPick;
//    public GameObject Picks;
//    public GameObject MoneyText;
//    public GameObject pickPriceText;

//    void Start()
//    {
//        EventManager.Instance.AddListener(EVENT_TYPE.PickUp, this);
//        MoneyText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Money}";
//        pickPriceText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Instance.NextPick?.GetComponent<Pick>().price}";

//        if (GameStates.Instance.NextPick == null)
//        {
//            CurrentPick.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
//            pickPriceText.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0.5f);

//            CurrentPick.GetComponent<Image>().sprite = Picks.transform.GetChild(GameStates.PickLvl - 1).GetComponent<Pick>().sprite;

//            NewPick.GetComponent<Image>().sprite = Picks.transform.GetChild(GameStates.PickLvl ).GetComponent<Pick>().sprite;
//        }
//        else
//        {
//            CurrentPick.GetComponent<Image>().sprite = Picks.transform.GetChild(GameStates.PickLvl).GetComponent<Pick>().sprite;

//            NewPick.GetComponent<Image>().sprite = Picks.transform.GetChild(GameStates.PickLvl + 1).GetComponent<Pick>().sprite;
//        }

//    }

//    public void BuyPick()
//    {
//        if (GameStates.Instance.NextPick != null &&
//            GameStates.Money >= GameStates.Instance.NextPick.GetComponent<Pick>().price)
//        {
//            GameStates.Money -= GameStates.Instance.NextPick.GetComponent<Pick>().price;
//            EventManager.Instance.PostNotification(EVENT_TYPE.BuyPick, this);
//        }
        
//    }

//    public void OpenShop()
//    {
//        EventManager.Instance.PostNotification(EVENT_TYPE.OpenShop, this);
//    }

//    public void CloseShop()
//    {
//        EventManager.Instance.PostNotification(EVENT_TYPE.CloseShop, this);
//    }

//    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
//    {
//        switch (Event_Type)
//        {
//            case EVENT_TYPE.PickUp:

//                if (GameStates.Instance.NextPick == null)
//                {
//                    CurrentPick.GetComponent<Image>().color = new Color(255,255,255,0.5f);
//                    pickPriceText.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0.5f);

//                    return;
//                }


//                if (GameStates.PickLvl < Picks.transform.childCount)
//                {
//                    CurrentPick.GetComponent<Image>().sprite = Picks.transform.GetChild(GameStates.PickLvl).GetComponent<Pick>().sprite;
//                }

//                if (GameStates.PickLvl < Picks.transform.childCount - 1)
//                {
//                    NewPick.GetComponent<Image>().sprite = Picks.transform.GetChild(GameStates.PickLvl + 1).GetComponent<Pick>().sprite;
//                }

//                MoneyText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Money}";
//                pickPriceText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Instance.NextPick.GetComponent<Pick>().price}";

//                break;
//        }
//    }

//}
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickShop : MonoBehaviour, IListener
{
    public GameObject CurrentPick;
    public GameObject NewPick;
    public GameObject Picks;
    public GameObject MoneyText;
    public GameObject pickPriceText;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.PickUp, this);
        MoneyText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Instance.money}";
        pickPriceText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Instance.NextPick?.GetComponent<Pick>().price}";

        if (GameStates.Instance.NextPick == null)
        {
            CurrentPick.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
            pickPriceText.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0.5f);

            CurrentPick.GetComponent<SpriteRenderer>().sprite = Picks.transform.GetChild(GameStates.Instance.PickLvl - 1).GetComponent<Pick>().sprite;

            NewPick.GetComponent<SpriteRenderer>().sprite = Picks.transform.GetChild(GameStates.Instance.PickLvl ).GetComponent<Pick>().sprite;
        }
        else
        {
            CurrentPick.GetComponent<SpriteRenderer>().sprite = Picks.transform.GetChild(GameStates.Instance.PickLvl).GetComponent<Pick>().sprite;

            NewPick.GetComponent<SpriteRenderer>().sprite = Picks.transform.GetChild(GameStates.Instance.PickLvl + 1).GetComponent<Pick>().sprite;
        }

    }

    public void BuyPick()
    {
        if (GameStates.Instance.NextPick != null &&
            GameStates.Instance.money >= GameStates.Instance.NextPick.GetComponent<Pick>().price)
        {
            GameStates.Instance.money -= GameStates.Instance.NextPick.GetComponent<Pick>().price;
            EventManager.Instance.PostNotification(EVENT_TYPE.BuyPick, this);
        }
        
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.PickUp:

                if (GameStates.Instance.NextPick == null)
                {
                    CurrentPick.GetComponent<SpriteRenderer>().color = new Color(255,255,255,0.5f);
                    pickPriceText.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0.5f);

                    return;
                }


                if (GameStates.Instance.PickLvl < Picks.transform.childCount)
                {
                    CurrentPick.GetComponent<SpriteRenderer>().sprite = Picks.transform.GetChild(GameStates.Instance.PickLvl).GetComponent<Pick>().sprite;
                }

                if (GameStates.Instance.PickLvl < Picks.transform.childCount - 1)
                {
                    NewPick.GetComponent<SpriteRenderer>().sprite = Picks.transform.GetChild(GameStates.Instance.PickLvl + 1).GetComponent<Pick>().sprite;
                }

                MoneyText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Instance.money}";
                pickPriceText.GetComponent<TextMeshProUGUI>().text = $"$ {GameStates.Instance.NextPick.GetComponent<Pick>().price}";

                break;
        }
    }

}
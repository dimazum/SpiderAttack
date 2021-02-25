using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogShopPanel : MonoBehaviour, IListener
{
    [SerializeField]
    private Image _logPanelImage;

    [SerializeField]
    private Animator _shopAnimator;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.Buy, this);
    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.Buy:
            {
                if (Param == null) return;
                var purchaseModel = (PurchaseModel)Param;
                _logPanelImage.sprite = purchaseModel.sprite;
                _shopAnimator.Play("shopLogRise");

                break;
            }
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{
    [SerializeField]
    private Transform purchaseCotainer;
    public ItemsData2 itemsData;
    public Image image;
    public TextMeshProUGUI priceText;
    private int _productIndex;
    private List<PurchaseModel> _purchaseList = new List<PurchaseModel>();
    [SerializeField]
    private Transform openShopBtn;

    public int ProductIndex
    {
        get => _productIndex;
        set
        {
            _productIndex = value;
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            openShopBtn.localPosition = Vector3.zero;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            openShopBtn.localPosition = new Vector3(0, 0, -500);
        }
    }

    private void PopulatePurchaseList()
    {
        for (int i = 0; i < purchaseCotainer.childCount; i++)
        {
            _purchaseList.Add(purchaseCotainer.GetChild(i).GetComponent<PurchaseModel>());
        }
    }

    void Start()
    {
        PopulatePurchaseList();
        SetProducts(0);
    }

    public void SetProducts(int index)
    {
        image.sprite = _purchaseList[index].sprite;
        priceText.text = _purchaseList[index].price.ToString();
    }

    public void NextProduct()
    {
        if(_productIndex < _purchaseList.Count - 1)
        {
            ProductIndex++;
            SetProducts(ProductIndex);
        }
       
    }
    public void PreviousProduct()
    {
        if (_productIndex > 0)
        {
            ProductIndex--;
            SetProducts(ProductIndex);
        }
    }

    public void Buy()
    {
        var purchase = _purchaseList[ProductIndex];

        if (purchase.price > GameStates.Money)
        {
            Debug.Log("Not enough money");
            EventManager.Instance.PostNotification(EVENT_TYPE.NotEnoughMoney, this);
            return;
        }
       
        
        var itemGroupIndex = (int)purchase.itemGroup;
        var itemCategoryIndex = purchase.category;
        itemsData.collections2[itemGroupIndex].itemTypes[itemCategoryIndex].Qty++;
        GameStates.Money -= purchase.price;
        EventManager.Instance.PostNotification(EVENT_TYPE.ChangeMoney, this, GameStates.Money);
        EventManager.Instance.PostNotification(EVENT_TYPE.Buy, this);
    }
}

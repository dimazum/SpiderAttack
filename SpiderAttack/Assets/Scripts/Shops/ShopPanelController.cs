using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{
    public string name;
    public Image image;
    public TextMeshProUGUI priceText;
    private int _productIndex;
    public ShopData shopData;
    public InventoryData itemsData;

    public int ProductIndex
    {
        get => _productIndex;
        set
        {
            if (value < shopData.shopItems.Length && value >= 0)
            {
                _productIndex = value;
            }
        }
    }

    void Start()
    {
        SetProducts(0);
    }

    public void SetProducts(int index)
    {
        image.sprite = shopData.shopItems[index].image;
        priceText.text = shopData.shopItems[index].price.ToString();
    }

    public void NextProduct()
    {
        ++ProductIndex;
        SetProducts(ProductIndex);
    }
    public void PreviousProduct()
    {
        --ProductIndex;
        SetProducts(ProductIndex);
    }

    public void Buy()
    {
        var itemGroupIndex = (int) shopData.shopItems[ProductIndex].ItemGroup;
        var itemCategoryIndex =  shopData.shopItems[ProductIndex].ItemCategoryNumber;
        itemsData.collections[itemGroupIndex].itemTypes[itemCategoryIndex].Qty++;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mime;
using Assets.Scripts.Utils.enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Shop/Data", fileName = "shoData")]
public class ShopData : ScriptableObject
{
    public ShopItem[] shopItems;
}

[System.Serializable]
public class ShopItem
{
    public int price;
    public Sprite image;
    public ItemGroup ItemGroup;
    public int ItemCategoryNumber;
}




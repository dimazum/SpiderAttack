using Assets.Scripts.Utils.enums;
using UnityEngine;

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
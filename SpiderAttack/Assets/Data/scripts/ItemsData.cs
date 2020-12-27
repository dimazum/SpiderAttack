using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Scripts.Utils.enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Data", fileName = "data")]
public class ItemsData : ScriptableObject
{
    public TypeCollection[] collections;
}

[System.Serializable]
public class TypeCollection
{
    public ItemGroup ItemGroup;
    public ItemType[] itemTypes;
}

[System.Serializable]
public class ItemType
{
    [SerializeField] private int _qty;
    public ItemCategory ItemCategory;
    public int Qty
    {
        get => _qty;
        set
        {
            if (value > 0)
            {
                _qty = value;
            }
        }
    }
    public bool endlesQty;
    public Sprite image;
    public GameObject prefab;
}

//снаряды: стрелы, бомбы
//ресурсы:20шт
//рецепты
//свитки телепортации, фонари, лестницы, бомбы


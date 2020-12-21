using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Data", fileName = "data")]
public class ItemsData : ScriptableObject
{
    public List<ItemType> collections;
}
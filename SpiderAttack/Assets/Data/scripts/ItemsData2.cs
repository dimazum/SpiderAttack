using Assets.Scripts.Utils.enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemsData2 : MonoBehaviour
{
    public TypeCollection2[] collections2;
}


[System.Serializable]
public class TypeCollection2
{
    public ItemGroup ItemGroup;
    public BaseItemType[] itemTypes;
}



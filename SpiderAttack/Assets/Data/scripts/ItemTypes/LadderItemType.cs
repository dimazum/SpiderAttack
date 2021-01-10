using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LadderItemType : ActionItemType, ISavableOnMap
{
    [SerializeField] private short _mapIndex;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}

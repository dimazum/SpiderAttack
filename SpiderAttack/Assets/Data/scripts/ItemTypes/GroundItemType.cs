using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemType : BaseItemType, ISavableOnMap
{
    public GroundCategory groundCategory;

    [SerializeField] private short _mapIndex;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}

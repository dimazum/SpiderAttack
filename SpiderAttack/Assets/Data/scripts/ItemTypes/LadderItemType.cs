using UnityEngine;

public class LadderItemType : ActionItemType, ISavableOnMap
{
    [SerializeField] private short _mapIndex;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}

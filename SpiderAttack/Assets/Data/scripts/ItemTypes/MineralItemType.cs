using UnityEngine;

public class MineralItemType : BaseItemType, ISavableOnMap
{
    public MineralCategory mineralCategory;

    [SerializeField] private short _mapIndex;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}

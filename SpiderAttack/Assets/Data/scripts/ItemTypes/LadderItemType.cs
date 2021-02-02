using UnityEngine;

public class LadderItemType : ActionItemType, ISavableOnMap, IPurchasable
{
    [SerializeField]
    private short _mapIndex;
    [SerializeField]
    private int _price;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
    public int Price
    { 
        get => _price;
        set => _price = value;
    }
}

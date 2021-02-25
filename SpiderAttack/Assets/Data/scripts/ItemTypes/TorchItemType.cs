using UnityEngine;

public class TorchItemType : ActionItemType, IPurchasable, ISavableOnMap
{
    [SerializeField]
    private int _price;
    [SerializeField]
    private short _mapIndex;
    public int Price
    {
        get => _price;
        set => _price = value;
    }

    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}
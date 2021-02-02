using UnityEngine;

public class BombItemType : ActionItemType, IPurchasable
{
    [SerializeField]
    private int _price;
    public int Price
    {
        get => _price;
        set => _price = value;
    }
}

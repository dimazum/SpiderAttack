using UnityEngine;

public class TeleportItemType : ActionItemType, IPurchasable
{
    [SerializeField]
    private int _price;
    public int Price
    {
        get => _price;
        set => _price = value;
    }
}

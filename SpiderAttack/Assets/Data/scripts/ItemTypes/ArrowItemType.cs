using UnityEngine;

public class ArrowItemType : BaseItemType, ICanBeInStock, ISavableOnMap
{
    [SerializeField]private int _qtyInStock;
    [SerializeField] private short _mapIndex;
    public ArrowCategory arrowCategory;

    public int QtyInStock 
    {
        get => _qtyInStock; 
        set => _qtyInStock = value;  
    }
    
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}

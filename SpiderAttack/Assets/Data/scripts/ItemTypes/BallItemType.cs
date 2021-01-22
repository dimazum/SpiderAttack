
using UnityEngine;

public class BallItemType : BaseItemType, ICanBeInStock, ISavableOnMap
{
    public BallCategory ballCategory;
    public int QtyInStock { get ; set ; }

    [SerializeField] private short _mapIndex;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}

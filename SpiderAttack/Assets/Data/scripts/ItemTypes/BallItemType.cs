
using UnityEngine;

public class BallItemType : BaseItemType, ICanBeInStock//, ISavableOnMap
{
    [SerializeField]
    private int _qtyInStock;
    public BallCategory ballCategory;
    public int QtyInStock
    {
        get => _qtyInStock;
        set
        {
            if (value >= 0)
            {
                _qtyInStock = value;

                if (SaveManager.InventoryItems.ContainsKey(_id))
                {
                    var saveInfo = SaveManager.InventoryItems[_id];
                    saveInfo.QtyInStock = _qtyInStock;
                }
                else
                {
                    SaveManager.InventoryItems[_id] = new SaveInfo { QtyInStock = _qtyInStock };
                }
            }
        }
    }

    //[SerializeField] private short _mapIndex;
    //public short MapIndex
    //{
    //    get => _mapIndex;
    //    set => _mapIndex = value;
    //}

    new void Awake()
    {
        base.Awake();
        if (SaveManager.InventoryItems.ContainsKey(_id))
        {
            QtyInStock = SaveManager.InventoryItems[_id].QtyInStock;
        }
    }
}

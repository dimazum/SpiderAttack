using UnityEngine;

public class MineralItemType : BaseItemType, ISavableOnMap, ICanBeInStock
{
    public MineralCategory mineralCategory;
    [SerializeField] private int _qtyInStock;

    [SerializeField] private short _mapIndex;
    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
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
                    SaveManager.InventoryItems[_id] = saveInfo;
                }
                else
                {
                    SaveManager.InventoryItems[_id] = new SaveInfo { QtyInStock = _qtyInStock };
                }
            }
        }
    }

    new void Awake()
    {
        base.Awake();
        if (SaveManager.InventoryItems.ContainsKey(_id))
        {
            QtyInStock = SaveManager.InventoryItems[_id].QtyInStock;
        }
    }
}

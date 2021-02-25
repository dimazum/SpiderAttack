using UnityEngine;

public abstract class BaseItemType : MonoBehaviour
{
    [SerializeField] 
    private int _qty;
    public int _id;
    public int Qty
    {
        get => _qty;
        set
        {
            if (value >= 0)
            {
                _qty = value;

                if (SaveManager.InventoryItems.ContainsKey(_id))
                {
                    var saveInfo = SaveManager.InventoryItems[_id];
                    saveInfo.Qty = _qty;
                    SaveManager.InventoryItems[_id] = saveInfo;
                }
                else
                {
                    SaveManager.InventoryItems[_id] = new SaveInfo { Qty = _qty };
                }
            }
        }
    }
    public bool endlesQty;
    public Sprite image;

    protected void Awake()
    {
        _id = GetInstanceID();
        if (SaveManager.InventoryItems.ContainsKey(_id))
        {
            Qty = SaveManager.InventoryItems[_id].Qty;
        }
    }
}

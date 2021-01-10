using UnityEngine;

public class BaseItemType : MonoBehaviour
{
    [SerializeField] 
    private int _qty;
    private int _id;
    public int Qty
    {
        get => _qty;
        set
        {
            if (value >= 0)
            {
                _qty = value;
                SaveManager.InventoryItems[_id] = _qty;
            }
        }
    }
    public bool endlesQty;
    public Sprite image;

    private void Start()
    {
        _id = GetInstanceID();
        if (SaveManager.InventoryItems.ContainsKey(_id))
        {
            Qty = SaveManager.InventoryItems[_id];
        }
    }
}

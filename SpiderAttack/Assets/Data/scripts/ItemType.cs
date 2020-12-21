using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemType", fileName = "new item collection")]
public class ItemType : ScriptableObject
{
    [SerializeField]private int _qty;
    public int Id;
    public string itemName;
    public Sprite image;
    
    public ItemCategory itemCategory;
    public int Qty
    {
        get => _qty;
        set
        {
            if (value < 0)
            {
                _qty = 0;
            }
        }
    }
}


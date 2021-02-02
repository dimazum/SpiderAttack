using Assets.Scripts.Utils.enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNotActiveItemsController : MonoBehaviour
{
    [SerializeField]
    private ItemsData2 itemsData;
    [SerializeField]
    private Transform notActiveCells;
    [SerializeField]
    private List<ItemGroup> notActiveGroup;
    private List<BaseItemType> notActiveItems;

    // Start is called before the first frame update
    void Start()
    {
        notActiveItems = new List<BaseItemType>();
        RenderNotActiveCells();
    }

    public void RenderNotActiveCells()
    {
        notActiveItems.Clear();
        foreach (var group in notActiveGroup)
        {
            var col = itemsData.collections2[(int)group];

            foreach (var itemType in col.itemTypes)
            {
                //if (itemType is ICanBeInStock type)
                //{
                //    if (!itemType.endlesQty && (itemType.Qty - type.QtyInStock) > 0)
                //    {
                //        notActiveItems.Add(itemType);
                //    }
                //}
                //else

                //{
                    if(!itemType.endlesQty && itemType.Qty > 0)
                    {
                        notActiveItems.Add(itemType);
                    }
                    
                //}
            }
        }

        for (int i = 0; i < notActiveCells.childCount; i++)
        {
            notActiveCells.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }


        for (int i = 0; i < notActiveCells.childCount && i < notActiveItems.Count; i++)
        {
            //var qtyInStock = (notActiveItems.ElementAtOrDefault(i) as ICanBeInStock)?.QtyInStock;
            //var qty = notActiveItems.ElementAtOrDefault(i)?.Qty;
            //var inventoryQty = qty;
            //if (qtyInStock.HasValue)
            //{
            //    inventoryQty = qty.Value - qtyInStock.Value;
            //}

            //if (inventoryQty > 0)
           // {
                notActiveCells.GetChild(i).GetChild(0).gameObject.SetActive(true);
                notActiveCells.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    notActiveItems.ElementAtOrDefault(i)?.image;
                notActiveCells.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = notActiveItems[i].Qty.ToString();
            //}
        }
    }
}

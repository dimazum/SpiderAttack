using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private int _activeInventoryIndex;
    public ItemsData2 itemsData;
    public Transform notActiveCells;
    public Transform activeCells;
    public List<ItemGroup> activeGroup;
    public List<ItemGroup> notActiveGroup; 
    private List<BaseItemType> activeItems;
    private List<BaseItemType> notActiveItems;

    public Button backpackBtn;
    public Transform outline;

    public int ActiveInventoryIndex
    {
        get => _activeInventoryIndex;
        set
        {
            if (value == activeItems.Count)
            {
                _activeInventoryIndex = 0;
                return;
            }

            _activeInventoryIndex = value;
        }
    }

    void Start()
    {
        activeItems = new List<BaseItemType>();
        notActiveItems = new List<BaseItemType>();
        RenderNotActiveCells();
        RenderActiveCells();
        if (activeItems.Count != 0)
        {
            backpackBtn.onClick.AddListener(((ActionItemType)activeItems.First()).characterAction.Invoke);
        }

        SetActiveItem(0);
        gameObject.SetActive(false);
    }

    private void RenderNotActiveCells()
    {
        notActiveItems.Clear();
        foreach (var group in notActiveGroup)
        {
            var col = itemsData.collections2[(int)group];

            foreach (var itemType in col.itemTypes)
            {
                if (itemType is ICanBeInStock type)
                {
                    if (!itemType.endlesQty && (itemType.Qty - type.QtyInStock) > 0)
                    {
                        notActiveItems.Add(itemType);
                    }
                }
                else
                {
                    notActiveItems.Add(itemType);
                }
            }
        }

        for (int i = 0; i < notActiveCells.childCount && i < notActiveItems.Count; i++)
        {
            var qtyInStock = (notActiveItems.ElementAtOrDefault(i) as ICanBeInStock)?.QtyInStock;
            var qty = notActiveItems.ElementAtOrDefault(i)?.Qty;
            var inventoryQty = qty;
            if (qtyInStock.HasValue)
            {
                inventoryQty = qty.Value - qtyInStock.Value;
            }

            if (inventoryQty > 0)
            {
                notActiveCells.GetChild(i).GetChild(0).gameObject.SetActive(true);
                notActiveCells.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    notActiveItems.ElementAtOrDefault(i)?.image;
                notActiveCells.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = inventoryQty.ToString();
            }
        }
    }

    private void RenderActiveCells()
    {
        activeItems.Clear();
        foreach (var group in activeGroup)
        {
            var col = itemsData.collections2[(int)group];
            activeItems.AddRange(col.itemTypes.Where(x=>!x.endlesQty));
        }

        for (int i = 0; i < activeCells.childCount && i < activeItems.Count; i++)
        {
            activeCells.GetChild(i).GetChild(0).gameObject.SetActive(true);
            activeCells.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                activeItems.ElementAtOrDefault(i)?.image; 
            activeCells.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = activeItems.ElementAtOrDefault(i)?.Qty.ToString();
        }
    }

    public void NextActiveItem()
    {
        ActiveInventoryIndex++;
        SetActiveItem(ActiveInventoryIndex);
    }

    private void SetActiveItem(int index)
    {
        outline.SetParent(activeCells.GetChild(index));
        outline.transform.localPosition = Vector3.zero;
        backpackBtn.transform.GetChild(0).GetComponent<Image>().sprite = activeItems[index].image;
        SetActionToBtn();
    }

    private void SetActionToBtn()
    {
        backpackBtn.onClick.RemoveAllListeners();
        backpackBtn.onClick.AddListener(((ActionItemType)activeItems[ActiveInventoryIndex]).characterAction.Invoke);
    }

    public void InventoryToggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            RenderNotActiveCells();
            RenderActiveCells();
            gameObject.transform.localPosition = Vector3.zero;
        }
    }
}

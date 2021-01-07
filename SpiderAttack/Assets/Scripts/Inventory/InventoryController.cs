using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour, IListener
{
    private int _activeInventoryIndex;
    public InventoryData itemsData;
    public Transform notActiveCells;
    public Transform activeCells;
    public List<ItemGroup> activeGroup;
    public List<ItemGroup> notActiveGroup; 
    private List<ItemType> activeItems;
    private List<ItemType> notActiveItems;
    public bool isInventoryEnabled;

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
        EventManager.Instance.AddListener(EVENT_TYPE.GetResurs, this);
        isInventoryEnabled = gameObject.activeSelf;
        activeItems = new List<ItemType>();
        notActiveItems = new List<ItemType>();
        RenderNotActiveCells();
        RenderActiveCells();
        backpackBtn.onClick.AddListener(activeItems.First().action.Invoke);
        SetActiveItem(0);
        gameObject.SetActive(false);
    }

    private void RenderNotActiveCells()
    {
        notActiveItems.Clear();
        foreach (var group in notActiveGroup)
        {
            var col = itemsData.collections[(int)group];
            notActiveItems.AddRange(col.itemTypes.Except(col.itemTypes.Where(x=>x.endlesQty || (x.Qty-x.QtyInStock)<1)));
        }

        for (int i = 0; i < notActiveCells.childCount && i < notActiveItems.Count ; i++)
        {
           
            var inventoryQty = notActiveItems.ElementAtOrDefault(i)?.Qty - notActiveItems.ElementAtOrDefault(i)?.QtyInStock;
            if(inventoryQty > 0)
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
            var col = itemsData.collections[(int)group];
            activeItems.AddRange(col.itemTypes.Except(col.itemTypes.Where(x => x.endlesQty)));
        }

        for (int i = 0; i < activeCells.childCount && i < activeItems.Count; i++)
        {
            activeCells.GetChild(i).GetChild(0).gameObject.SetActive(true);
            activeCells.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                activeItems.ElementAtOrDefault(i)?.image; 
            activeCells.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = activeItems.ElementAtOrDefault(i)?.Qty.ToString();
            //activeItems.ElementAtOrDefault(i)?.action.Invoke();
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
        backpackBtn.onClick.AddListener(activeItems[ActiveInventoryIndex].action.Invoke);
    }

    public void InventoryToggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            RenderNotActiveCells();
            RenderActiveCells();
            gameObject.transform.localPosition = new Vector3(0, 0, 20000);
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //switch (Event_Type)
        //{
        //    case EVENT_TYPE.GetResurs:
        //        RenderNotActiveCells();
        //        break;
        //}
    }
}

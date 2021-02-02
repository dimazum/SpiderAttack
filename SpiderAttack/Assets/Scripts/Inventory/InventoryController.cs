using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour, IListener
{
    [SerializeField]
    private GameObject _inventoryPanel;
    private InventoryNotActiveItemsController _inventoryNotActiveItemsController;
    private int _activeInventoryIndex;
    [SerializeField]
    private ItemsData2 itemsData;
    [SerializeField]
    private Transform activeCells;
    [SerializeField]
    private List<ItemGroup> activeGroup;
    public List<BaseItemType> activeItems;
    [SerializeField]
    private Button backpackBtn;
    [SerializeField]
    private Transform outline;
    [SerializeField]
    private TextMeshProUGUI qtyText;
    [SerializeField]
    private Sprite transparentMock;

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
        _inventoryNotActiveItemsController = FindObjectOfType<InventoryNotActiveItemsController>();
        EventManager.Instance.AddListener(EVENT_TYPE.Buy, this);
        activeItems = new List<BaseItemType>();
        RenderActiveCells();
        RenderQtyText(activeItems.ElementAtOrDefault(ActiveInventoryIndex)?.Qty.ToString());

        SetActiveItem(0);
        //gameObject.SetActive(false);
    }

    private void RenderActiveCells()
    {
        activeItems.Clear();
        foreach (var group in activeGroup)
        {
            var col = itemsData.collections2[(int)group];
            activeItems.AddRange(col.itemTypes.Where(x => x.Qty > 0));
        }

        for (int i = 0; i < activeCells.childCount; i++)
        {
            activeCells.GetChild(i).GetChild(0).gameObject.SetActive(false);
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
        if(activeItems.Count < 1)
        {
            backpackBtn.transform.GetChild(0).GetComponent<Image>().sprite = transparentMock;
            SetActionToBtn(null);
            RenderQtyText(string.Empty);
            return;
        }


        outline.SetParent(activeCells.GetChild(index));
        outline.transform.localPosition = Vector3.zero;
        backpackBtn.transform.GetChild(0).GetComponent<Image>().sprite = activeItems[index].image;
        SetActionToBtn(((ActionItemType)activeItems[index]).characterAction);
        RenderQtyText(activeItems[index].Qty.ToString());
    }

    private void SetActionToBtn(UnityEvent unityEvent)
    {
        backpackBtn.onClick.RemoveAllListeners();

        if(unityEvent!= null)
        {
            backpackBtn.onClick.AddListener(unityEvent.Invoke);
        }
    }

    private void RenderQtyText(string text)
    {
        qtyText.text = text;
    }

    public void UseItem()
    {
        activeItems[ActiveInventoryIndex].Qty--;
        RenderQtyText(activeItems[ActiveInventoryIndex].Qty.ToString());

        if(activeItems[ActiveInventoryIndex].Qty < 1)
        {
            activeItems.Remove(activeItems[ActiveInventoryIndex]);
            ActiveInventoryIndex = 0;
            SetActiveItem(ActiveInventoryIndex);
        }
    }

    public void InventoryToggle()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
        if (_inventoryPanel.activeSelf)
        {
            _inventoryNotActiveItemsController.RenderNotActiveCells();
            RenderActiveCells();
            RenderQtyText(activeItems.ElementAtOrDefault(ActiveInventoryIndex)?.Qty.ToString());
            //gameObject.transform.localPosition = Vector3.zero;
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.Buy:
                RenderActiveCells();
                RenderQtyText(activeItems[ActiveInventoryIndex].Qty.ToString());
                break;
        }
    }
}

using Assets.Scripts.Utils.enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBoxController : MonoBehaviour
{
    public GameObject ammoBoxPanel;
    public List<BaseItemType> ammoList;
    public List<ItemGroup> ammoGroups;
    public ItemsData2 itemsData;
    public Transform cellsContainer;
    [SerializeField]
    private Transform boxBtn;
    void Start()
    {

        ammoList = new List<BaseItemType>();
        RenderAmmoCells();
    }
    // Start is called before the first frame update
    private void RenderAmmoCells()
    {
        ammoList.Clear();
        foreach (var group in ammoGroups)
        {
            var col = itemsData.collections2[(int)group];
            ammoList.AddRange(col.itemTypes.Where(x => (x as ICanBeInStock)?.QtyInStock > 0));
        }

        for (int i = 0; i < cellsContainer.childCount && i < ammoList.Count; i++)
        {

            var ammoQty = (ammoList.ElementAtOrDefault(i) as ICanBeInStock)?.QtyInStock;
            if (ammoQty > 0)
            {
                cellsContainer.GetChild(i).GetChild(0).gameObject.SetActive(true);
                cellsContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    ammoList.ElementAtOrDefault(i)?.image;
                cellsContainer.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ammoQty.ToString();
            }
        }
    }

    public void AmmoBoxEnable()
    {
        ammoBoxPanel.SetActive(true);
        RenderAmmoCells();
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            StartCoroutine(ReplaceResources());
            boxBtn.localPosition = Vector3.zero;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {

            boxBtn.localPosition = new Vector3(0, 0, -500);
        }
    }

    private IEnumerator ReplaceResources()
    {
        foreach (var group in ammoGroups)
        {
            var col = itemsData.collections2[(int)group];

            foreach (var itemType in col.itemTypes)
            {
                if(itemType is MineralItemType type)
                {
                    if (type.Qty> 0 && type.mineralCategory == MineralCategory.Gold && type.Qty > 0 )
                    {
                        GameStates.Money += 500 * type.Qty;
                        EventManager.Instance.PostNotification(EVENT_TYPE.ChangeMoney, this, GameStates.Money);
                        itemType.Qty = 0;
                        continue;
                    }
                        
                }
                ((ICanBeInStock)itemType).QtyInStock += itemType.Qty;
                itemType.Qty = 0;
            }
        }
        yield return null;
    }
}

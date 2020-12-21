using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public ItemsData itemsData;
    public GameObject cellContainer;

    void Start()
    {
        RenderCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RenderCells()
    {
        for (int i = 0; i < cellContainer.transform.childCount; i++)
        {
            cellContainer.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemsData.collections.ElementAtOrDefault(i)?.image;
            cellContainer.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = itemsData.collections.ElementAtOrDefault(i)?.Qty.ToString();

        }
    }
}

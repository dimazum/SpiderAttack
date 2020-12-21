using MarchingBytes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsInventotyController : MonoBehaviour, IListener
{
    public Image arrowTypeSelectImage;
    public ItemsData itemsData;
    public ItemType constantType; //arrowX1
    private Ballista ballista;
    public List<ItemType> arrowTypes;

    int typeCounter = 0;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
        ballista = FindObjectOfType<Ballista>();
        arrowTypes.Add(constantType);
        arrowTypeSelectImage.sprite = arrowTypes[0].image;

        PopulateArrowTypes();
    }

    private void PopulateArrowTypes()
    {
        foreach (var item in itemsData.collections)
        {
            if(item == null)
            {
                break;
            }

            if (item is ArrowType)
            {
                if (item.Qty > 0)
                {
                    arrowTypes.Add(item);
                }
                
            }
        }

    }

    public void NextArrowType()
    {
        if(typeCounter  < arrowTypes.Count -1)
        {
            typeCounter++;
        }
        else
        {
            typeCounter = 0;
        }

        SetItenCategotyToPool(arrowTypes[typeCounter].itemCategory);
        RenderArrowTypeIcon(typeCounter);

    }

    void SetItenCategotyToPool(ItemCategory itemCategory)
    {
        ballista.itemCategory = itemCategory;
    }

    public void RenderArrowTypeIcon(int counter)
    {
        arrowTypeSelectImage.sprite = arrowTypes[counter].image;
        if (!((ArrowType)arrowTypes[counter]).endlessQty)
        {
            arrowTypeSelectImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = arrowTypes[counter].Qty.ToString();
        }
        else
        {
            arrowTypeSelectImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.BallistaShot:
                if (Param == null) break;

                var arrowType = (ItemCategory)Param;
                var collection = itemsData.collections.Where(x=>x!=null).FirstOrDefault(x => x.itemCategory == arrowType);
                if(collection != null)
                {
                    collection.Qty--;
                    if (collection.Qty < 1)
                    {
                        arrowTypes.Remove(collection);
                        typeCounter = 0;
                        SetItenCategotyToPool(0);//set default
                        RenderArrowTypeIcon(0);
                        break;
                    }

                    RenderArrowTypeIcon(typeCounter);
                    
                }
                
                break;

        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletsInventoryController : MonoBehaviour, IListener
{
    //public Image bulletTypeSelectImage;
    //public ItemsData itemsData;
    //public ItemType constantType; //arrowX1
    //private Trebuchet _trebuchet;
    //public List<ItemType> bulletTypes;

    //int _typeCounter = 0;

    //void Start()
    //{
    //    EventManager.Instance.AddListener(EVENT_TYPE.TrebShot, this);
    //    _trebuchet = FindObjectOfType<Trebuchet>();
    //    bulletTypes.Add(constantType);
    //    bulletTypeSelectImage.sprite = bulletTypes[0].image;

    //    PopulateArrowTypes();
    //}

    //private void PopulateArrowTypes()
    //{
    //    foreach (var item in itemsData.collections)
    //    {
    //        if (item == null)
    //        {
    //            break;
    //        }

    //        if (item is BallType)
    //        {
    //            if (item.Qty > 0)
    //            {
    //                bulletTypes.Add(item);
    //            }

    //        }
    //    }

    //}


    //public void NextBulletType()
    //{
    //    if (_typeCounter < bulletTypes.Count - 1)
    //    {
    //        _typeCounter++;
    //    }
    //    else
    //    {
    //        _typeCounter = 0;
    //    }

    //    SetItemCategoryToPool(bulletTypes[_typeCounter].itemCategory);
    //    RenderBulletTypeIcon(_typeCounter);
    //}

    //void SetItemCategoryToPool(ItemCategory itemCategory)
    //{
    //    _trebuchet.itemCategory = itemCategory;
    //}

    //public void RenderBulletTypeIcon(int typeCounter)
    //{
    //    bulletTypeSelectImage.sprite = bulletTypes[typeCounter].image;
    //    if (!((BallType)bulletTypes[typeCounter]).endlessQty)
    //    {
    //        bulletTypeSelectImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = bulletTypes[typeCounter].Qty.ToString();
    //    }
    //    else
    //    {
    //        bulletTypeSelectImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
    //    }
    //}

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //switch (Event_Type)
        //{
        //    case EVENT_TYPE.TrebShot:
        //        if (Param == null) break;

        //        var arrowType = (ItemCategory)Param;
        //        var collection = itemsData.collections.Where(x => x != null).FirstOrDefault(x => x.itemCategory == arrowType);
        //        if (collection != null)
        //        {
        //            collection.Qty--;
        //            if (collection.Qty < 1)
        //            {
        //                bulletTypes.Remove(collection);
        //                _typeCounter = 0;
        //                SetItemCategoryToPool(0);//set default
        //                RenderBulletTypeIcon(0);
        //                break;
        //            }

        //            RenderBulletTypeIcon(_typeCounter);

        //        }

        //        break;

        //}
    }
}

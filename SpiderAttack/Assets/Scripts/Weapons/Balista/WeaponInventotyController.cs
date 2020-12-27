using System;
using MarchingBytes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils.enums;
using Assets.Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventotyController : MonoBehaviour, IListener
{
    public Image arrowTypeSelectImage;
    public ItemsData itemsData;
    private TypeCollection _bulletCollection;
    private IWeapon weapon;
    int _typeCounter;
    public ItemGroup ItemGroup;


    void Start()
    {
        _bulletCollection = itemsData.collections[(int)ItemGroup];
       
        if (ItemGroup == ItemGroup.Arrows)
        {
            weapon = FindObjectOfType<Ballista>();
            EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
        }

        if (ItemGroup == ItemGroup.Balls)
        {
            weapon = FindObjectOfType<Trebuchet>();
            EventManager.Instance.AddListener(EVENT_TYPE.TrebShot, this);
        }

        arrowTypeSelectImage.sprite = _bulletCollection.itemTypes[0].image;
    }

    public void NextArrowType()
    {
        if (_typeCounter < _bulletCollection.itemTypes.Length - 1)
        {
            _typeCounter++;
            if (_bulletCollection.itemTypes[_typeCounter].Qty < 1)
            {
                _typeCounter++;
            }
        }
        else
        {
            _typeCounter = 0;
        }

        SetItenCategotyToPool(_bulletCollection.itemTypes[_typeCounter].ItemCategory);
        RenderArrowTypeIcon(_typeCounter);

    }

    void SetItenCategotyToPool(ItemCategory itemCategory)
    {
        weapon.itemCategory = itemCategory;
    }

    public void RenderArrowTypeIcon(int counter)
    {
        arrowTypeSelectImage.sprite = _bulletCollection.itemTypes[counter].image;
        if (!(_bulletCollection.itemTypes[counter].endlesQty))
        {
            arrowTypeSelectImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _bulletCollection.itemTypes[counter].Qty.ToString();
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
            case EVENT_TYPE.BallistaShot :
 
                if (_bulletCollection.itemTypes[(int)ItemGroup].Qty < 1)
                {
                    SetItenCategotyToPool(_bulletCollection.itemTypes[_typeCounter].ItemCategory);
                }
                _bulletCollection.itemTypes[_typeCounter].Qty--;

                if (_bulletCollection.itemTypes[_typeCounter].Qty < 1)
                {
                    _typeCounter = 0;
                }
                RenderArrowTypeIcon(_typeCounter);

                break;

            case EVENT_TYPE.TrebShot:



                if (_bulletCollection.itemTypes[(int)ItemGroup].Qty < 1)
                {
                    SetItenCategotyToPool(_bulletCollection.itemTypes[_typeCounter].ItemCategory);
                }
                _bulletCollection.itemTypes[_typeCounter].Qty--;

                if (_bulletCollection.itemTypes[_typeCounter].Qty < 1)
                {
                    _typeCounter = 0;
                }
                RenderArrowTypeIcon(_typeCounter);

                break;
        }
    }
}

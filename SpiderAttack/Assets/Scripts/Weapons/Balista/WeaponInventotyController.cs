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
    public InventoryData itemsData;
    private TypeCollection _bulletCollection;
    private IWeapon weapon;
    int _typeCounter;

    public int typeCounter
    {
        get => _typeCounter;
        set
        {
            if (value < _bulletCollection.itemTypes.Length)
            {
                _typeCounter = value;

            }
            else
            {
                _typeCounter = 0;
            }
        }
    }
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
        typeCounter++;
        while (_bulletCollection.itemTypes[typeCounter].Qty < 1)
        {
            typeCounter++;
        }


        SetItenCategotyToPool(_bulletCollection.itemTypes[typeCounter].ItemCategory);
        RenderArrowTypeIcon(typeCounter);

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
    private void ChangeQty()
    {
        if (_bulletCollection.itemTypes[typeCounter].endlesQty) 
        {
            return;
        }

        _bulletCollection.itemTypes[typeCounter].Qty--;

        if (_bulletCollection.itemTypes[typeCounter].Qty < 1)
        {
            typeCounter = 0;
            SetItenCategotyToPool(_bulletCollection.itemTypes[0].ItemCategory);
            RenderArrowTypeIcon(0);
        }
        RenderArrowTypeIcon(typeCounter);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.BallistaShot:
                if (ItemGroup == ItemGroup.Arrows)
                {
                    ChangeQty();
                }

                break;

            case EVENT_TYPE.TrebShot:
                if (ItemGroup == ItemGroup.Balls)
                {
                    ChangeQty();
                }
                break;
        }
    }
}

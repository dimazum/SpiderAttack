using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventotyController : MonoBehaviour
{
    private int _typeCounter;
    protected TypeCollection _bulletCollection;
    public ItemGroup ItemGroup;
    public Image arrowTypeSelectImage;
    public ItemsData2 itemsData;

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


    protected void Start()
    {
        _bulletCollection = itemsData.collections2[(int)ItemGroup];
        arrowTypeSelectImage.sprite = _bulletCollection.itemTypes[0].image;
    }

    public void NextArrowType()
    {
        typeCounter++;
        while (_bulletCollection.itemTypes[typeCounter].Qty < 1)
        {
            typeCounter++;
        }

        SetItemCategoryToPool(typeCounter);
        RenderArrowTypeIcon(typeCounter);
    }


    protected virtual void SetItemCategoryToPool(int typeIndex) { }


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
    protected void ChangeQty()
    {
        if (_bulletCollection.itemTypes[typeCounter].endlesQty) 
        {
            return;
        }

        _bulletCollection.itemTypes[typeCounter].Qty--;

        if (_bulletCollection.itemTypes[typeCounter].Qty < 1)
        {
            typeCounter = 0;
            SetItemCategoryToPool(0);
            RenderArrowTypeIcon(0);
        }
        RenderArrowTypeIcon(typeCounter);
    }
}

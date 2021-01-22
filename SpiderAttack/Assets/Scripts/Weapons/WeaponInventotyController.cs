using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventotyController : MonoBehaviour
{
    private int _typeCounter;
    public ItemGroup ItemGroup;
    [SerializeField]
    private Image arrowTypeSelectImage;
    [SerializeField]
    private ItemsData2 itemsData;
    protected TypeCollection _bulletCollection;

    public int TypeCounter
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
        TypeCounter++;
        while (_bulletCollection.itemTypes[TypeCounter].Qty < 1)
        {
            TypeCounter++;
        }

        SetItemCategoryToPool(TypeCounter);
        RenderArrowTypeIcon(TypeCounter);
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
        if (_bulletCollection.itemTypes[TypeCounter].endlesQty) 
        {
            return;
        }

        _bulletCollection.itemTypes[TypeCounter].Qty--;

        if (_bulletCollection.itemTypes[TypeCounter].Qty < 1)
        {
            TypeCounter = 0;
            SetItemCategoryToPool(0);
            RenderArrowTypeIcon(0);
        }
        RenderArrowTypeIcon(TypeCounter);
    }
}

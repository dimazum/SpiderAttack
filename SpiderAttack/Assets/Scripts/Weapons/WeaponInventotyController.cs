using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponInventotyController : MonoBehaviour
{
    public int _typeCounter;
    public ItemGroup ItemGroup;
    [SerializeField]
    private ItemsData2 itemsData;
    protected TypeCollection _bulletCollection;
    [SerializeField]
    private TextMeshProUGUI textQty;

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
    }

    public void NextArrowType()
    {
        TypeCounter++;
        while (((ICanBeInStock)_bulletCollection.itemTypes[TypeCounter]).QtyInStock < 1)
        {
            TypeCounter++;
        }

        SetItemCategoryToPool(TypeCounter);
        RenderArrowTypeIcon(TypeCounter);
        RenderQty(TypeCounter);
    }

    protected virtual void SetItemCategoryToPool(int typeIndex) { }

    protected void DisableObjects(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }

    private void RenderQty(int counter)
    {
        if (!(_bulletCollection.itemTypes[counter].endlesQty))
        {
            textQty.text = ((ICanBeInStock)_bulletCollection.itemTypes[counter]).QtyInStock.ToString();
        }
        else
        {
            textQty.text = string.Empty;
        }
    }

    public abstract void RenderArrowTypeIcon(int counter);

    protected void ChangeQty()
    {
        if (_bulletCollection.itemTypes[TypeCounter].endlesQty) 
        {
            return;
        }

        ((ICanBeInStock)_bulletCollection.itemTypes[TypeCounter]).QtyInStock--;

        if (((ICanBeInStock)_bulletCollection.itemTypes[TypeCounter]).QtyInStock < 1)
        {
            TypeCounter = 0;
            SetItemCategoryToPool(0);
            RenderArrowTypeIcon(0);
            
        }
        RenderArrowTypeIcon(TypeCounter);
        RenderQty(TypeCounter);
    }
}

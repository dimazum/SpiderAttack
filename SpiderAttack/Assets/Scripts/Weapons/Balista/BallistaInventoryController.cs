using UnityEngine;

public class BallistaInventoryController : WeaponInventotyController, IListener
{
    private IArrowUsingWeapon _arrowUsingWeapon;
    [SerializeField]
    private GameObject[] imageSprites;

    new void Start()
    {
        base.Start();
        _arrowUsingWeapon = FindObjectOfType<Ballista>();
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
    }

    protected override void SetItemCategoryToPool(int typeIndex)
    {
        _arrowUsingWeapon.ArrowCategory = ((ArrowItemType)_bulletCollection.itemTypes[typeIndex]).arrowCategory;
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.BallistaShot:
                ChangeQty();

                    break;
        }
    }

    public override void RenderArrowTypeIcon(int counter)
    {
        var category = ((ArrowItemType)_bulletCollection.itemTypes[TypeCounter]).arrowCategory;

        DisableObjects(imageSprites);
        if (category == ArrowCategory.ArrowX1)
        {
            imageSprites[0].SetActive(true);

        }
        if (category == ArrowCategory.ArrowX2)
        {
            imageSprites[1].SetActive(true);
        }
        if (category == ArrowCategory.ArrowX3)
        {
            imageSprites[2].SetActive(true);
        }
        if (category == ArrowCategory.ArrowX5)
        {
            imageSprites[3].SetActive(true);
        }
    }
}

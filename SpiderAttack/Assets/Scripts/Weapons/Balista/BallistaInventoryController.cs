using UnityEngine;

public class BallistaInventoryController : WeaponInventotyController, IListener
{
    private IArrowUsingWeapon _arrowUsingWeapon;

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

}

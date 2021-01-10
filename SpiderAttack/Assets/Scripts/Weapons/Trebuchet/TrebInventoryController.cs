using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapons;
using UnityEngine;

public class TrebInventoryController : WeaponInventotyController, IListener
{
    private IBallUsingWeapon _arrowUsingWeapon;

    new void Start()
    {
        base.Start();
        _arrowUsingWeapon = FindObjectOfType<Trebuchet>();
        EventManager.Instance.AddListener(EVENT_TYPE.TrebShot, this);
    }

    protected override void SetItemCategoryToPool(int typeIndex)
    {
        _arrowUsingWeapon.BallCategory = ((BallItemType)_bulletCollection.itemTypes[typeIndex]).ballCategory;
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.TrebShot:
                ChangeQty();
                break;
        }
    }
}

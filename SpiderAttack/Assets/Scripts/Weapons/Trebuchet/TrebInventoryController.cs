using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrebInventoryController : WeaponInventotyController, IListener
{
    private IBallUsingWeapon _arrowUsingWeapon;

    [SerializeField]
    private GameObject[] imageSprites;

    [SerializeField]
    private GameObject[] ballSprites;


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

   

    public override void RenderArrowTypeIcon(int counter)
    {
        var category = ((BallItemType)_bulletCollection.itemTypes[TypeCounter]).ballCategory;
        DisableObjects(ballSprites);
        DisableObjects(imageSprites);
        if (category == BallCategory.BallX1 )
        {
            imageSprites[0].SetActive(true);
            ballSprites[0].SetActive(true);
        }
        if (category == BallCategory.BallX3)
        {
            imageSprites[1].SetActive(true);
            ballSprites[1].SetActive(true);
        }
        if (category == BallCategory.BallX5)
        {
            imageSprites[2].SetActive(true);
            ballSprites[2].SetActive(true);
        }
        if (category == BallCategory.BallSuper)
        {
            imageSprites[3].SetActive(true);
            ballSprites[3].SetActive(true);
        }
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

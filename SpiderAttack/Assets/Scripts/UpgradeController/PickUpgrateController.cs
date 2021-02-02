using UnityEngine;

public class PickUpgrateController : BaseUpgrateController
{
    [SerializeField]
    private SpriteRenderer charakterPick;
    [SerializeField]
    private Transform firstGrounds;
    [SerializeField]
    private Transform secondGrounds;

    void Start()
    {
        RenderMainImages(GameStates.PickLvl);
        RenderCharPick(GameStates.PickLvl);
        RenderPrice(GameStates.PickLvl);
        RenderGroundType(GameStates.PickLvl);
    }

    public override void Upgrate()
    {
        GameStates.PickLvl++;
        RenderMainImages(GameStates.PickLvl);
        RenderCharPick(GameStates.PickLvl);
        RenderPrice(GameStates.PickLvl);
        RenderGroundType(GameStates.PickLvl);
    }

    protected override void RenderMainImages(int lvl)
    {
        firstShopImage.sprite = models.GetChild(lvl).GetComponent<PickUpgrateModel>().sprite;
        if (GameStates.PickLvl < 4)
        {
            secondShopImage.sprite = models.GetChild(lvl + 1).GetComponent<PickUpgrateModel>().sprite;
        }
        else
        {
            secondPanel.SetActive(false);
            firstPanel.localPosition = new Vector3(0, 18, 0);
            pricePanel.SetActive(false);
        }
    }

    protected override void RenderPrice(int lvl)
    {
        if (GameStates.PickLvl < 4)
        {
            priceText.text = models.GetChild(lvl + 1).GetComponent<PickUpgrateModel>().price.ToString();
        }
    }

    private void RenderGroundType(int lvl)
    {
        for (int i = 0; i <= lvl; i++)
        {
            firstGrounds.GetChild(i).gameObject.SetActive(true);
        }

        if (GameStates.PickLvl < 4)
        {
            for (int i = 0; i <= lvl + 1; i++)
            {
                secondGrounds.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void RenderCharPick(int lvl)
    {
        charakterPick.sprite = models.GetChild(lvl).GetComponent<PickUpgrateModel>().sprite;
    }
}

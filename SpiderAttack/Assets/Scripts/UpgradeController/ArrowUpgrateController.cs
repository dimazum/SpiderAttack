using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUpgrateController : BaseUpgrateController
{
    [SerializeField]
    private Transform pool;
    [SerializeField]
    private Image[] UIimages;
    [SerializeField]
    private SpriteRenderer ballistaArrowImage;


    void Start()
    {
        RenderMainImages(GameStates.ArrowLvl);
        RenderPrice(GameStates.ArrowLvl);
        RefreshBallsInPool(GameStates.ArrowLvl);
        RenderUIImages(GameStates.ArrowLvl);
        RenderBallistaImage(GameStates.ArrowLvl);
        RenderAttributeText(GameStates.ArrowLvl);
    }

    public override void Upgrate()
    {
        GameStates.ArrowLvl++;
        RenderMainImages(GameStates.ArrowLvl);
        RenderPrice(GameStates.ArrowLvl);
        RefreshBallsInPool(GameStates.ArrowLvl);
        RenderUIImages(GameStates.ArrowLvl);
        RenderBallistaImage(GameStates.ArrowLvl);
        RenderAttributeText(GameStates.ArrowLvl);
    }

    protected override void RenderPrice(int lvl)
    {
        if (GameStates.ArrowLvl < 6)
        {
            priceText.text = models.GetChild(lvl + 1).GetComponent<ArrowUpgrateModel>().price.ToString();
        }
    }

    protected override void RenderMainImages(int lvl)
    {
        firstShopImage.sprite = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().sprite;
        if (GameStates.ArrowLvl < 6)
        {
            secondShopImage.sprite = models.GetChild(lvl + 1).GetComponent<ArrowUpgrateModel>().sprite;
        }
        else
        {
            secondPanel.SetActive(false);
            firstPanel.localPosition = new Vector3(0, 18, 0);
            pricePanel.SetActive(false);
        }
    }

    void RenderAttributeText(int lvl)
    {
        firstAttributeText.text = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().damage.ToString();
        if (GameStates.ArrowLvl < 6)
        {
            secondAttributeText.text = models.GetChild(lvl + 1).GetComponent<ArrowUpgrateModel>().damage.ToString();
        }
    }

    void RenderUIImages(int lvl)
    {
        for (int i = 0; i < UIimages.Length; i++)
        {
            UIimages[i].sprite = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().sprite;
        }
    }

    void RenderBallistaImage(int lvl)
    {
        ballistaArrowImage.sprite = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().sprite;
    }

    

    private void RefreshBallsInPool(int lvl)
    {
        var arrowSprite = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().sprite;
        for (int i = 0; i < pool.childCount; i++)
        {
            //pool.GetChild(i).GetComponent<Bullet>().Damage = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().damage;

            for (int j = 0; j < pool.GetChild(i).childCount; j++)
            {
                pool.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().sprite = arrowSprite;
                pool.GetChild(i).GetChild(j).GetComponent<Bullet>().Damage = models.GetChild(lvl).GetComponent<ArrowUpgrateModel>().damage;
            }
        }
    }   
}
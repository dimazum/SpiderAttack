using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUpgradeController : BaseUpgradeController
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

    public override void Upgrade()
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
            priceText.text = models.GetChild(lvl + 1).GetComponent<ArrowUpgradeModel>().price.ToString();
        }
    }

    protected override void RenderMainImages(int lvl)
    {
        firstShopImage.sprite = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().sprite;
        if (GameStates.ArrowLvl < 6)
        {
            secondShopImage.sprite = models.GetChild(lvl + 1).GetComponent<ArrowUpgradeModel>().sprite;
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
        firstAttributeText.text = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().damage.ToString();
        if (GameStates.ArrowLvl < 6)
        {
            secondAttributeText.text = models.GetChild(lvl + 1).GetComponent<ArrowUpgradeModel>().damage.ToString();
        }
    }

    void RenderUIImages(int lvl)
    {
        for (int i = 0; i < UIimages.Length; i++)
        {
            UIimages[i].sprite = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().sprite;
        }
    }

    void RenderBallistaImage(int lvl)
    {
        ballistaArrowImage.sprite = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().sprite;
    }

    

    private void RefreshBallsInPool(int lvl)
    {
        var arrowSprite = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().sprite;
        for (int i = 0; i < pool.childCount; i++)
        {
            //pool.GetChild(i).GetComponent<Bullet>().Damage = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().damage;

            for (int j = 0; j < pool.GetChild(i).childCount; j++)
            {
                pool.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().sprite = arrowSprite;
                pool.GetChild(i).GetChild(j).GetComponent<Bullet>().Damage = models.GetChild(lvl).GetComponent<ArrowUpgradeModel>().damage;
            }
        }
    }   
}
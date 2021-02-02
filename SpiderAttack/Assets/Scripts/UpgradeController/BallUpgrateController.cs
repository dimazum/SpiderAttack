using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallUpgrateController : MonoBehaviour
{
    [SerializeField]
    private Image firstShopImage;
    [SerializeField]
    private Image secondShopImage;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI firstDamageText;
    [SerializeField]
    private TextMeshProUGUI secondDamageText;
    [SerializeField]
    private Transform ballModels;
    [SerializeField]
    private Transform ballPool;
    [SerializeField]
    private Image[] UIimages;
    [SerializeField]
    private SpriteRenderer[] trebuchetImages;
    [SerializeField]
    private GameObject secondPanel;
    [SerializeField]
    private RectTransform firstPanel;
    [SerializeField]
    private GameObject pricePanel;
    [SerializeField]
    private Transform openUpgrateShopBtn;

    void Start()
    {
        RenderBallsImages(GameStates.BallLvl);
        RenderPrice(GameStates.BallLvl);
        RefreshBallsInPool(GameStates.BallLvl);
        RenderUIImages(GameStates.BallLvl);
        RenderTrebuchetImages(GameStates.BallLvl);
        RenderDamageText(GameStates.BallLvl);
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            openUpgrateShopBtn.localPosition = Vector3.zero;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            openUpgrateShopBtn   .localPosition = new Vector3(0, 0, -500);
        }
    }

    void RenderBallsImages(int lvl)
    {
        firstShopImage.sprite = ballModels.GetChild(lvl).GetComponent<BallUpgrateModel>().sprite;
        if (GameStates.BallLvl < 6)
        {
            secondShopImage.sprite = ballModels.GetChild(lvl + 1).GetComponent<BallUpgrateModel>().sprite;
        }
        else
        {
            secondPanel.SetActive(false);
            firstPanel.localPosition = new Vector3(0, 18, 0);
            pricePanel.SetActive(false);
        }
    }

    void RenderDamageText(int lvl)
    {
        firstDamageText.text = ballModels.GetChild(lvl).GetComponent<BallUpgrateModel>().damage.ToString();
        if (GameStates.BallLvl < 6)
        {
            secondDamageText.text = ballModels.GetChild(lvl + 1).GetComponent<BallUpgrateModel>().damage.ToString();
        }
    }

    void RenderUIImages(int lvl)
    {
        for (int i = 0; i < UIimages.Length; i++)
        {
            UIimages[i].sprite = ballModels.GetChild(lvl).GetComponent<BallUpgrateModel>().sprite;
        }
    }

    void RenderTrebuchetImages(int lvl)
    {
        for (int i = 0; i < trebuchetImages.Length; i++)
        {
            trebuchetImages[i].sprite = ballModels.GetChild(lvl).GetComponent<BallUpgrateModel>().sprite;
        }
    }

    void RenderPrice(int lvl)
    {
        if (GameStates.BallLvl < 6)
        {
            priceText.text = ballModels.GetChild(lvl + 1).GetComponent<BallUpgrateModel>().price.ToString();
        }
    }

    private void RefreshBallsInPool(int lvl)
    {
        var ballSprite = ballModels.GetChild(lvl).GetComponent<BallUpgrateModel>().sprite;
        for (int i = 0; i < ballPool.childCount; i++)
        {
            ballPool.GetChild(i).GetComponent<Bullet>().Damage = ballModels.GetChild(lvl).GetComponent<BallUpgrateModel>().damage;
            var category = ballPool.GetChild(i).GetComponent<BaseBall>().ballCategory;

            if(category == BallCategory.BallX1 || category == BallCategory.BallSuper)
            {
                ballPool.GetChild(i).GetComponent<SpriteRenderer>().sprite = ballSprite;
            }

            if (category == BallCategory.BallX3)
            {
                ballPool.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ballSprite;
                ballPool.GetChild(i).transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ballSprite;
                ballPool.GetChild(i).transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = ballSprite;
            }

            if (category == BallCategory.BallX5)
            {
                ballPool.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ballSprite;
                ballPool.GetChild(i).transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ballSprite;
                ballPool.GetChild(i).transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = ballSprite;
                ballPool.GetChild(i).transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = ballSprite;
                ballPool.GetChild(i).transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = ballSprite;
            }
        }
    }

    public void UpgrateTrebBall()
    {
        GameStates.BallLvl++;
        RenderBallsImages(GameStates.BallLvl);
        RenderPrice(GameStates.BallLvl);
        RefreshBallsInPool(GameStates.BallLvl);
        RenderUIImages(GameStates.BallLvl);
        RenderTrebuchetImages(GameStates.BallLvl);
        RenderDamageText(GameStates.BallLvl);
    }
}

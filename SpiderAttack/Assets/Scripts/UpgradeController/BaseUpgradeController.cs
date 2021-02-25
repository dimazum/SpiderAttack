using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUpgradeController : MonoBehaviour
{
    [SerializeField]
    protected Image firstShopImage;
    [SerializeField]
    protected Image secondShopImage;
    [SerializeField]
    protected TextMeshProUGUI priceText;
    [SerializeField]
    protected TextMeshProUGUI firstAttributeText;
    [SerializeField]
    protected TextMeshProUGUI secondAttributeText;
    [SerializeField]
    protected Transform models;
    [SerializeField]
    protected RectTransform firstPanel;
    [SerializeField]
    protected GameObject secondPanel;
    [SerializeField]
    protected GameObject pricePanel;

    protected abstract void RenderPrice(int lvl);
    protected abstract void RenderMainImages(int lvl);
    public abstract void Upgrade();
}

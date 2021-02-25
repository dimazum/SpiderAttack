using UnityEngine;

public class GatesUpgradeController : BaseUpgradeController
{
    private GateRightController _gateRightController;
    void Start()
    {
        _gateRightController = FindObjectOfType<GateRightController>();
        RenderPrice(GameStates.GateLvl);
        RenderAttributeText(GameStates.GateLvl);
        SetGateHp(GameStates.GateLvl);
    }

    public override void Upgrade()
    {
        GameStates.GateLvl++;
        EventManager.Instance.PostNotification(EVENT_TYPE.GateUp, this);
        RenderPrice(GameStates.GateLvl);
        RenderAttributeText(GameStates.GateLvl);
        SetGateHp(GameStates.GateLvl);
    }

    protected override void RenderMainImages(int lvl)
    {
        throw new System.NotImplementedException();
    }

    protected override void RenderPrice(int lvl)
    {
        if (GameStates.GateLvl < 12)
        {
            priceText.text = models.GetChild(lvl + 1).GetComponent<GateUpgradeModel>().price.ToString();
        }
    }

    private void RenderAttributeText(int lvl)
    {
        firstAttributeText.text = models.GetChild(lvl).GetComponent<GateUpgradeModel>().hp.ToString();
        if (GameStates.GateLvl < 12)
        {
            secondAttributeText.text = models.GetChild(lvl + 1).GetComponent<GateUpgradeModel>().hp.ToString();
        }
        else
        {
            secondPanel.SetActive(false);
            firstPanel.localPosition = new Vector3(0, 18, 0);
            pricePanel.SetActive(false);
        }
    }

    private void SetGateHp(int lvl)
    {
        var gateMaxHp = models.GetChild(lvl).GetComponent<GateUpgradeModel>().hp;
        //_gateRightController.gateCurrentHp = gateMaxHp;
        GameStates.GateMaxHP = gateMaxHp;
        _gateRightController.SetMaxGateHP(gateMaxHp);
    }
}

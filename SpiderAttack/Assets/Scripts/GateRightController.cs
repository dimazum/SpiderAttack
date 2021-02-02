using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GateRightController : MonoBehaviour, IListener
{
    private Animator _animator;
    public Slider gateHpSlider;//??
    public GameObject door;
    private bool _isDestroyed;
    [SerializeField]
    private TextMeshProUGUI currentHpText;
    [SerializeField]
    private TextMeshProUGUI maxHpText;
    [SerializeField]
    private GameObject fixGateBtn;


    void Start()
    {
        _animator = GetComponent<Animator>();
        SetMaxGateHP(GameStates.GateMaxHP);
        SetCurrentGateHP(GameStates.GateCurrentHP);

        if(GameStates.GateCurrentHP < GameStates.GateMaxHP)
        {
            fixGateBtn.SetActive(true);
        }

        EventManager.Instance.AddListener(EVENT_TYPE.SpiderMeleeAttackGate, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderWebReachedWall, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SpiderMeleeAttackGate:
                _animator.Play("gate_right_Hit");
                if (Param != null)
                {
                    ChangeGateHp((int)Param);
                }
                break;

            case EVENT_TYPE.SpiderWebReachedWall:
                _animator.Play("web");
                if (Param != null)
                {
                    ChangeGateHp((int)Param);
                }
                break;

            case EVENT_TYPE.StartDay:
                if (GameStates.GateCurrentHP < GameStates.GateMaxHP)
                {
                    fixGateBtn.SetActive(true);
                }
                break;

            case EVENT_TYPE.StartNight:
                fixGateBtn.SetActive(false);
                break;
        }
    }

    public void FixGate()
    {
        if (GameStates.Money <= 0)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.NotEnoughMoney, this);
            return;
        }
        var fixValue = GameStates.GateMaxHP - GameStates.GateCurrentHP;
        int div;
        //= GameStates.Money >= fixValue ? fixValue : fixValue - GameStates.Money;
        if (GameStates.Money >= fixValue)
        {
            div = fixValue;
            fixGateBtn.SetActive(false);
        }
        else
        {
            div = GameStates.Money;

        }

        GameStates.Money -= div;
        EventManager.Instance.PostNotification(EVENT_TYPE.ChangeMoney, this, GameStates.Money);
        GameStates.GateCurrentHP += div;
        SetCurrentGateHP(GameStates.GateCurrentHP);
        EventManager.Instance.PostNotification(EVENT_TYPE.Buy, this);
    }

    public void SetMaxGateHP(int maxHp)
    {
        gateHpSlider.maxValue = maxHp;
        maxHpText.text = maxHp.ToString();
    }

    public void SetCurrentGateHP(int currnetHp)
    {
        gateHpSlider.value = currnetHp;
        currentHpText.text = currnetHp.ToString();
    }

    private void ChangeGateHp(int damage)
    {
        GameStates.GateCurrentHP -= damage;
        SetCurrentGateHP(GameStates.GateCurrentHP);

        if (GameStates.GateCurrentHP <= 0 && !_isDestroyed)
        {
            _isDestroyed = true;
            EventManager.Instance.PostNotification(EVENT_TYPE.GateDestroy,this);
            door.SetActive(false);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}

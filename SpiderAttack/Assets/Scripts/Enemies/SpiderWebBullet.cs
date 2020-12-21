using System.Linq;
using UnityEngine;

public class SpiderWebBullet : MonoBehaviour, IListener
{
    private BattleController battleController;
    private MainHomeController mainHomeController;
    private Transform webContainer;
    private Transform spiderTarget;
    private bool _isAttack;
    private Vector3 zeroPos = Vector3.zero;

    public int PowerDamage { get; set; }
    public Spider spiderController;

    void Awake()
    {
        webContainer = transform.parent;
        battleController = FindObjectOfType<BattleController>();
        mainHomeController = FindObjectOfType<MainHomeController>();
        spiderController = spiderController.GetComponent<Spider>();
    }


    void Start()
    {
        spiderTarget = spiderController.FindTarget();
        PowerDamage = spiderController.rangeDamage;

        EventManager.Instance.AddListener(EVENT_TYPE.GateDestroy, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderSpitRangeAttack, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderStartRangeAttack, this);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GateRight"))
        {
            _isAttack = false;
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebReachedWall, this, PowerDamage);
            transform.SetParent(webContainer);
            transform.localPosition = zeroPos;
            spiderTarget = spiderController.FindTarget();
        }

        if (collision.CompareTag("player"))
        {
            _isAttack = false;
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebHitCharacter, this);
            transform.SetParent(webContainer);
            transform.localPosition = zeroPos;
            battleController.allTargets.Remove(battleController.allTargets.FirstOrDefault(x => x.name.Contains("Character")));

            spiderTarget = spiderController.FindTarget();
        }

        if (collision.CompareTag("mainHouse"))
        {
            _isAttack = false;
            spiderTarget.SetParent(null);
            mainHomeController.webHits++;
            spiderTarget = spiderController.FindTarget();
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebHitMainHouse, this);

            if (mainHomeController.webHits == 4)
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver, this);
                battleController.allTargets.Remove(battleController.allTargets.FirstOrDefault(x=>x.name.Contains("SpidertargetHouse")));
                spiderTarget = spiderController.FindTarget();
            }
        }
    }


    void Update()
    {
        if (_isAttack && spiderTarget!= null)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, spiderTarget.position, 20 * Time.deltaTime);
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SpiderStartRangeAttack:
                if (Sender == spiderController)
                {
                    _isAttack = false;
                    transform.SetParent(webContainer);
                    transform.localPosition = zeroPos;
                }
                break;

            case EVENT_TYPE.SpiderSpitRangeAttack:
                if(Sender == spiderController)
                {
                    transform.SetParent(null);
                    _isAttack = true;
                }
                
                break;

            case EVENT_TYPE.GateDestroy:
                transform.SetParent(webContainer);
                    transform.localPosition = zeroPos;

                    {
                        var spiderTargetGate = battleController.allTargets.FirstOrDefault(x => x.gameObject.name.Contains("spiderTargetGate"));
                        battleController.allTargets.Remove(spiderTargetGate);
                    }

                    spiderTarget = spiderController.FindTarget();
                

                break;
        }
    }


}

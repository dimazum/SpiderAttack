using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.enums;
using Unity.UNetWeaver;
using UnityEngine;

public class SpiderWebBullet : MonoBehaviour, IListener
{
    public BattleController battleController;
    public SpiderController spiderController;
    public Transform webContainer;
    public Transform spiderTarget2;
    public bool _isAttack;
    public int PowerDamage { get; set; }
    public List<GameObject> webs;

    int _webCount = 0;


    void Start()
    {
        spiderTarget2 = spiderController.FindTarget();
        PowerDamage = spiderController.rangeDamage;

        EventManager.Instance.AddListener(EVENT_TYPE.GateDestroy, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderSpitRangeAttack, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderStartRangeAttack, this);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GateRight")
        {
            _isAttack = false;
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebReachedWall, this, PowerDamage);
            transform.SetParent(webContainer);
            transform.localPosition = new Vector3(0, 0, 0);
            spiderTarget2 = spiderController.FindTarget();
        }

        if (collision.tag == "player")
        {
            _isAttack = false;
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebHitCharacter, this);
            transform.SetParent(webContainer);
            transform.localPosition = new Vector3(0, 0, 0);
            battleController.allTargets.Remove(battleController.spiderTarget);
            
            spiderTarget2 = spiderController.FindTarget();
        }

        if (collision.tag == "mainHouse")
        {
            spiderTarget2.SetParent(null);
            webs.ElementAtOrDefault(_webCount++)?.SetActive(true);
            spiderTarget2 = spiderController.FindTarget();

            if (_webCount == 4)
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.GameOver, this);
                battleController.allTargets.Remove(battleController.spiderTarget);
                spiderTarget2 = spiderController.FindTarget();
            }
        }
    }


    void Update()
    {
        if (_isAttack && spiderTarget2!= null)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, spiderTarget2.position, 20 * Time.deltaTime);
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SpiderStartRangeAttack:
                _isAttack = false;
                transform.SetParent(webContainer);
                transform.localPosition = new Vector3(0, 0, 0);

                break;
            case EVENT_TYPE.SpiderSpitRangeAttack:

                transform.SetParent(null);
                _isAttack = true;
                break;

            case EVENT_TYPE.GateDestroy:

                battleController.allTargets.Remove(battleController.spiderTarget);
                var spiderTarget = spiderController.FindTarget();

                break;
        }
    }


}

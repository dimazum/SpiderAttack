using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.enums;
using UnityEngine;

public class SpiderWebBullet : MonoBehaviour, IListener
{
    public SpiderController spiderController;
    public Transform character;
    public Transform webContainer;
    public Transform spiderTarget;
    public bool _isAttack;
    public int PowerDamage { get; set; }


    void Start()
    {
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
        }

        if (collision.tag == "player")
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebHitCharacter, this);
        }
    }



    void Update()
    {
        if (_isAttack)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, spiderTarget.position, 20 * Time.deltaTime);
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
                spiderTarget.position = character.position;
                break;
        }
    }


}

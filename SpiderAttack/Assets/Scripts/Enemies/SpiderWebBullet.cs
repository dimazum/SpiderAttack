using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.enums;
using Unity.UNetWeaver;
using UnityEngine;

public class SpiderWebBullet : MonoBehaviour, IListener
{
    public SpiderController spiderController;
    public Transform character;
    public Transform webContainer;
    public Transform spiderTarget;
    public Transform mainHouse;
    public bool _isAttack;
    public int PowerDamage { get; set; }
    public List<GameObject> webs;

    int webCount = 0;


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
            _isAttack = false;
            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderWebHitCharacter, this);
            transform.SetParent(webContainer);
            transform.localPosition = new Vector3(0, 0, 0);
            spiderTarget.position = mainHouse.position; 
            //spiderTarget.SetParent(null);
        }

        

        if (collision.tag == "mainHouse")
        {
            webs.ElementAtOrDefault(webCount++)?.SetActive(true);
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
                Debug.Log(Vector2.Distance(webContainer.position, character.position));
                Debug.Log(Vector2.Distance(mainHouse.position, webContainer.position));
                if (Vector2.Distance(webContainer.position, character.position) <
                    Vector2.Distance(mainHouse.position, webContainer.position))
                {
                    spiderTarget.position = character.position;
                    spiderTarget.SetParent(character);
                }
                else
                {
                    spiderTarget.position = mainHouse.position;
                    spiderTarget.SetParent(null);
                }
                
                break;
        }
    }


}

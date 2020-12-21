using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Monetization;

public class Rounds : MonoBehaviour, IListener
{
    public Round[] rounds;
    public Transform spawnSpiderPos;
    public Transform enemiesContainer;
    private readonly List<GameObject> _enemyList = new List<GameObject>();


    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderDie, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.StartNight:
                GameObject[] spiders = rounds[GameStates.Instance.round].Enemies;

                StartCoroutine(SpiderInstantiate(spiders));
                break;

            case EVENT_TYPE.SpiderDie:

                var deathSpider = (GameObject)Param;
                _enemyList.Remove(deathSpider);
                if (!_enemyList.Any())
                {
                    Debug.Log("You win");
                    EventManager.Instance.PostNotification(EVENT_TYPE.StartDay,this, GameStates.Instance.round++);
                }


                break;
        }
    }

    private IEnumerator SpiderInstantiate(GameObject[] spiders)
    {
        foreach (var spider in spiders.Where(x => x != null))
        {
            var enemy = Instantiate(spider, spawnSpiderPos.position, spawnSpiderPos.rotation, enemiesContainer);
            _enemyList.Add(enemy);
            yield return new WaitForSeconds(.5f);
        }
       
    }
}

[System.Serializable]
public struct Round
{
    public int duration;
    public GameObject[] Enemies;
}

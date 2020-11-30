using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainHomeController : MonoBehaviour, IListener
{
    public int webHits;
    public GameObject[] webs;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderWebHitMainHouse, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SpiderWebHitMainHouse:

                webs.ElementAtOrDefault(webHits - 1)?.SetActive(true);
                break;
        }
    }
}

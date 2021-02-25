using System.Linq;
using UnityEngine;

public class MainHomeController : MonoBehaviour, IListener
{
    [SerializeField]
    private Transform _mainHouseBtn;
    public GameObject[] webs;
    public int webHits;
    [SerializeField] 
    private GameObject _mainHousePanel;

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

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            _mainHouseBtn.localPosition = Vector3.zero;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            _mainHouseBtn.localPosition = new Vector3(0, 0, -500);
            _mainHousePanel.SetActive(false);
        }
    }
}

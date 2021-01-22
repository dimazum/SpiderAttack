using System.Collections;
using UnityEngine;

public class ThougtsController : MonoBehaviour, IListener
{
    [SerializeField]
    private GameObject charThoughts;
    private bool coIsStarted;
    [SerializeField]
    private Sprite[] picks;


    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.FullBackpack, this);
        EventManager.Instance.AddListener(EVENT_TYPE.NeedNextPick, this);

    }

    void Update()
    {
        
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.NeedNextPick:
                if (Param == null) return;
                var sprite = picks[(byte)Param];
                if (!coIsStarted)
                {
                   StartCoroutine(SetThoughts(sprite));
                }

                break;
        }
    }

    private IEnumerator SetThoughts(Sprite sprite) {
        coIsStarted = true;
        charThoughts.SetActive(true);
        charThoughts.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
        yield return new WaitForSeconds(1);
        charThoughts.SetActive(false);
        coIsStarted = false;
    }
}

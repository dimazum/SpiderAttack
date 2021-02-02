using UnityEngine;

public class RatingController : MonoBehaviour, IListener
{
    public bool _arrowHitTarget;
    //private int balistaRating = 5;
    //private int trebuchetRating = 10;
    public int rating;

    // Start is called before the first frame update
    void Start()
    {
       // rating = balistaRating;
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ArrowHitTarget, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ArrowMissedTarget, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.BallistaShot:
                {
                    break;
                }
            case EVENT_TYPE.ArrowHitTarget:
                {
                    _arrowHitTarget = true;
                    GameStates.rating += rating;
                    EventManager.Instance.PostNotification(EVENT_TYPE.ChangeRating, this);
                    CalculateRating();
                    
                   
                    break;
                }
            case EVENT_TYPE.ArrowMissedTarget:
                {
                    _arrowHitTarget = false;
                    CalculateRating();
                    break;
                }
        }
    }

    private void CalculateRating()
    {
        if (_arrowHitTarget)
        {
            if (rating >= 25) return;
            rating += 5;
        }
        else
        {
            rating = 0;
        }
    }
}

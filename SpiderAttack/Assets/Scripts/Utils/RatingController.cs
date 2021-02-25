using UnityEngine;

public class RatingController : MonoBehaviour, IListener
{
    public bool _arrowHitTarget;
    //private int balistaRating = 5;
    //private int trebuchetRating = 10;
    public int rating;
    private int _maxRating;

    // Start is called before the first frame update
    void Start()
    {
       // rating = balistaRating;
        EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ArrowHitTarget, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ArrowMissedTarget, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BallHitTarget, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CharacterEnterFirstFloor, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CharacterEnterSecondFloor, this);
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
                    CalculateRating(5, 25);

                    break;
                }

            case EVENT_TYPE.BallHitTarget:
            {
                _arrowHitTarget = true;
                GameStates.rating += rating;
                EventManager.Instance.PostNotification(EVENT_TYPE.ChangeRating, this);
                CalculateRating(10, _maxRating);

                break;
            }
            case EVENT_TYPE.ArrowMissedTarget:
                {
                    _arrowHitTarget = false;
                    CalculateRating(5, _maxRating);
                    break;
                }

            case EVENT_TYPE.CharacterEnterFirstFloor:
            {
                _maxRating = 50;
                break;
            }

            case EVENT_TYPE.CharacterEnterSecondFloor:
            {
                if (rating > 25)
                {
                    rating = 25;
                    EventManager.Instance.PostNotification(EVENT_TYPE.RatingAdditionUp, this, rating);
                }
                _maxRating = 25;
                    break;
            }
        }
    }

    private void CalculateRating(int rate, int maxRating)
    {
        if (_arrowHitTarget)
        {
            if (rating >= maxRating) return;
            rating += rate;
        }
        else
        {
            rating = 0;
        }

        if (rating > 50)
        {
            rating = 50;
        }

        EventManager.Instance.PostNotification(EVENT_TYPE.RatingAdditionUp, this, rating);

        if (rating == 50)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.MaxRatingAddition, this);
        }
    }
}

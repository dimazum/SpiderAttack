using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour, IListener
{
    private int defaultDayDuration = 600;
    //private float _sec ;
    private int _sec;
    public List<int> rounds;

    public TextMeshProUGUI textTime;

    private IEnumerator co;
    private readonly WaitForSeconds _secondDelay = new WaitForSeconds(1f);

    //private float currentTime;
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
        if (co != null)
        {
            StopCoroutine(co);
        }

        var day = GameStates.Instance.round;
        _sec = GameStates.Instance.currentTime;
        co = StartTimer(day, _sec);

        StartCoroutine(co);

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (co != null)
            {
                StopCoroutine(co);
            }

            co = StartTimer(GameStates.Instance.round);
           
            StartCoroutine(co);
            GameStates.Instance.round++;
        }

    }


    public IEnumerator StartTimer(int day, int sec = 0)
    {
        if (sec == 0)
        {
            GameStates.Instance.currentTime = rounds.ElementAtOrDefault(day) != 0 ? rounds.ElementAtOrDefault(day) : defaultDayDuration;
        }
        else
        {
            GameStates.Instance.currentTime = sec;
        }

        while (GameStates.Instance.currentTime > 0)
        {
            yield return _secondDelay;
            textTime.text = $"{GameStates.Instance.currentTime / 60}:{Mathf.Round(GameStates.Instance.currentTime % 60):00}";
            GameStates.Instance.currentTime -= 1;
        }
        EventManager.Instance.PostNotification(EVENT_TYPE.StartNight, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.StartDay:

                if (co != null)
                {
                    StopCoroutine(co);
                }
                co = StartTimer(GameStates.Instance.round);
                StartCoroutine(co);

                break;
        }
    }

    public void WaitForNight()
    {
        if (GameStates.Instance.currentTime > 10)
        {
            GameStates.Instance.currentTime = 10;
        }
    }
}


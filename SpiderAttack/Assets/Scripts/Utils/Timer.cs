using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour, IListener
{
    private readonly int defaultDayDuration = 600;
    private int _sec;
    [SerializeField]
    private TextMeshProUGUI textMinuteTime;
    [SerializeField]
    private TextMeshProUGUI textSecTimePart1;
    [SerializeField]
    private TextMeshProUGUI textSecTimePart2;
    private IEnumerator co;
    private readonly WaitForSeconds _secondDelay = new WaitForSeconds(1f);
    private Rounds _rounds;
    [SerializeField]
    private int _currentTime;
    private int tempOldVal;

    private Dictionary<int, string> Seconds { get; set; } = new Dictionary<int, string>() { };
    public int CurrentTime
    {
        get => _currentTime; 
        set => _currentTime = value; 
    }
    
    void Start()
    {
        _rounds = FindObjectOfType<Rounds>();
        GameStates.isDay = true;
        EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ResetTime, this);
        
        if (co != null)
        {
            StopCoroutine(co);
        }

        var day = GameStates.Round;
        _sec = GameStates.Instance.CurrentTime;
        co = StartTimer(day, _sec);

        StartCoroutine(co);
        PopulateSeconds();
    }

    private void PopulateSeconds()
    {
        for (int i = 0; i < 10; i++)
        {
            Seconds.Add(i, i.ToString());
        }
    }

    public IEnumerator StartTimer(int day, int sec = 0)
    {
        if (sec == 0)
        {
            CurrentTime = _rounds.rounds.ElementAtOrDefault(day).duration != 0 ? _rounds.rounds.ElementAtOrDefault(day).duration : defaultDayDuration;
        }
        else
        {
            CurrentTime = sec;
        }

        while (CurrentTime > 0)
        {
            yield return _secondDelay;
            CurrentTime -= 1;
            BuildTime();
        }
        EventManager.Instance.PostNotification(EVENT_TYPE.StartNight, this);
    }


    private void BuildTime()
    {
        var sec = CurrentTime % 60;
        
        if(tempOldVal != CurrentTime / 60)
        {
            textMinuteTime.text = $"{CurrentTime / 60}:";
            tempOldVal = CurrentTime / 60;
        }
        
        textSecTimePart1.text = Seconds[sec / 10];
        textSecTimePart2.text = Seconds[sec % 10];


    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.StartDay:
                {
                    if (co != null)
                    {
                        StopCoroutine(co);
                    }
                    co = StartTimer(GameStates.Round);
                    StartCoroutine(co);

                    GameStates.isDay = true;
                    break;
                }

            case EVENT_TYPE.StartNight:
                {
                    GameStates.isDay = false;
                    break;
                }

            case EVENT_TYPE.ResetTime:
                {
                    if (CurrentTime > 6)
                    {
                        CurrentTime = 6;
                    }
                    break;
                }
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int defaultDayDuration = 600;
    //private float _sec ;

    public List<int> rounds;

    public TextMeshProUGUI textTime;

    private IEnumerator co;

    //private float currentTime;
    void Start()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        co = StartTimer(GameStates.Instance.round, GameStates.Instance.currentTime);

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
            yield return new WaitForSeconds(1f);
            textTime.text = $"{GameStates.Instance.currentTime / 60}:{Mathf.Round(GameStates.Instance.currentTime % 60):00}";
            GameStates.Instance.currentTime -= 1;
        }
        EventManager.Instance.PostNotification(EVENT_TYPE.StartNight, this);
    }

   
}

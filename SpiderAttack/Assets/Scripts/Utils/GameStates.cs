using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : Singleton<GameStates>, IListener
{
    private int _pickLvl; //current pick lvl
    private bool _savesReset;
    public int PickLvl
    {
        get { return _pickLvl; }
        set
        {
            if (_pickLvl < Picks.childCount - 1)
            {
                _pickLvl = value;
            }
        }
    }

    public int money = 1000; //current money

    public int round = 0; // current day
    public int currentTime; //current timer
    public bool isDay;
    public bool inTrebuchetPlace;

    public Transform Picks;
    public GameObject CurrentPick;
    public GameObject NextPick;

    void Awake()
    {
        PickLvl = PlayerPrefs.GetInt("PickLvl", 0);
        currentTime = PlayerPrefs.GetInt("CurrentTime", 0);
        CurrentPick = GetPick(PickLvl);
        NextPick = GetPick(PickLvl + 1);
        EventManager.Instance.AddListener(EVENT_TYPE.BuyPick, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.BuyPick:
                PickLvl++;
                PlayerPrefs.SetInt("PickLvl", PickLvl);
                CurrentPick = GetPick(PickLvl);
                NextPick = GetPick(PickLvl + 1);
                EventManager.Instance.PostNotification (EVENT_TYPE.PickUp, this);
                break;
        }
    }

    private GameObject GetPick(int pickLvl)
    {
        return pickLvl < Picks.childCount  ? Picks.GetChild(pickLvl).gameObject : null;
    }


    public void SavesReset()
    {
        PlayerPrefs.SetInt("PickLvl", 0);
        PlayerPrefs.SetInt("CurrentTime", 0);
    }

    void OnApplicationQuit()
    {
        if (_savesReset)
        {
            return;
        }
        PlayerPrefs.SetInt("CurrentTime", currentTime);
        PlayerPrefs.SetInt("CurrentDay", round);
        //Debug.Log("Application ending after " + Time.time + " seconds");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _savesReset = true;
            SavesReset();
        }
    }
}

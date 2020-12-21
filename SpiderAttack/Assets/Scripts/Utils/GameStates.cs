using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UpgradeController;
using UnityEngine;

public class GameStates : Singleton<GameStates>, IListener
{
    private int _pickLvl; //current pick lvl
    private int _gateLvl; //current pick lvl
    private int _currentTime;
    public bool _inCity;
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
    public int GateLvl
    {
        get => _gateLvl;
        set
        {
            if (_gateLvl < 11)
            {
                _gateLvl = value;
            }
        }
    }
    public int CurrentTime
    {
        get
        {
            return _currentTime;
        }
        set
        {
            _currentTime = value;
        }
    } //current timer

    public int money = 1000; //current money

    public int round = 0; // current day

    public bool isDay;
    public bool inTrebuchetPlace;
    public bool inBallistaPlace;
    private Timer _timer;
    public bool InCity 
    {
        get => _inCity;
        set
        {
            _inCity = value;
            EventManager.Instance.PostNotification(EVENT_TYPE.CharInCity, this, _inCity);
        }
    }

    public float smoothCameraSpeed = 5;

    public Transform Picks;
    public GameObject CurrentPick;
    public GameObject NextPick;
    private GateUpgradeController _gateUpgradeController;

    void Awake()
    {
        
        PickLvl = PlayerPrefs.GetInt("PickLvl", 0);
        GateLvl = PlayerPrefs.GetInt("GateLvl", 0);
        CurrentTime = PlayerPrefs.GetInt("CurrentTimeInSeconds", 0);
        round = PlayerPrefs.GetInt("CurrentDay", 0);
        CurrentPick = GetPick(PickLvl);
        NextPick = GetPick(PickLvl + 1);
        
    }

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.BuyPick, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BuyGate, this);
        _timer = FindObjectOfType<Timer>();
        _gateUpgradeController = FindObjectOfType<GateUpgradeController>();
        InCity = true;
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

            case EVENT_TYPE.BuyGate:
                GateLvl++;
                PlayerPrefs.SetInt("GateLvl", GateLvl);
                EventManager.Instance.PostNotification(EVENT_TYPE.GateUp, this);
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
        PlayerPrefs.SetInt("GateLvl", 0);
        PlayerPrefs.SetInt("CurrentTimeInSeconds", 0);
        PlayerPrefs.SetInt("CurrentDay", 0);
    }

    void OnApplicationQuit()
    {
        if (_savesReset)
        {
            return;
        }
        PlayerPrefs.SetInt("CurrentTimeInSeconds", _timer.CurrentTime);
        PlayerPrefs.SetInt("CurrentDay", round);
        PlayerPrefs.SetInt("GateLvl", GateLvl);
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

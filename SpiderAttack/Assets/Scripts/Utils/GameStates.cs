using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : Singleton<GameStates>, IListener
{
    private int _pickLvl;
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

    public int money = 1000;

    public Transform Picks;
    public GameObject CurrentPick;
    public GameObject NextPick;

    void Awake()
    {
        PickLvl = PlayerPrefs.GetInt("PickLvl", 0);
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

    public void Reset()
    {
        PlayerPrefs.SetInt("PickLvl", 0);
    }
}

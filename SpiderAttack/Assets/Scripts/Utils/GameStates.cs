using UnityEngine;

public class GameStates : Singleton<GameStates>
{
    [SerializeField]
    private static int _pickLvl; //current pick lvl
    private static int _gateLvl; //current pick lvl
    private static int _ballLvl;
    private static int _arrowLvl;
    private int _currentTime;
    public static int _gateCurrentHp;
    public bool _inCity;
    public  static int _money;
    public static int rating;
    public static int PickLvl
    {
        get { return _pickLvl; }
        set
        {
            if (_pickLvl < 4)
            {
                _pickLvl = value;
            }
        }
    }
    public static int GateLvl
    {
        get => _gateLvl;
        set
        {
            if (_gateLvl < 12)
            {
                _gateLvl = value;
            }
        }
    }
    public static int GateMaxHP { get; set; }
    public static int GateCurrentHP {
        get => _gateCurrentHp;
        set
        {
            if (value >= 0)
            {
                _gateCurrentHp = value;
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
    public static int BallLvl {
        get => _ballLvl;
        set
        {
            if (_ballLvl < 6)
            {
                _ballLvl = value;
            }
        }
    }
    public static int ArrowLvl
    {
        get => _arrowLvl;
        set
        {
            if (_arrowLvl < 6)
            {
                _arrowLvl = value;
            }
        }
    }
    public bool IsGameOver;
    public static int Money {
        get => _money;
        set
        {
            _money = value;
        }
    }
    public static int Round { get; set; } // current day

    public static bool isDay;
    public bool inTrebuchetPlace;
    public bool inBallistaPlace;
    public bool InCity
    {
        get 
        {
            return _inCity;
        }

        set
        {
            _inCity = value;
        }
    }

    public float smoothCameraSpeed = 5;

}

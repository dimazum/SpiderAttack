using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveManager : MonoBehaviour, IListener
{
    private GameObject mainChar;
    [SerializeField]
    private List<short> map;
    public static Vector3 charPos;
    private ES3Settings _settings;
    private Timer _timer;
    public static Dictionary<int, SaveInfo> InventoryItems { get; set; } = new Dictionary<int, SaveInfo>();

    public List<short> Map
    {
        get => map;
        set => map = value;
    }
   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Clean savings");
            ES3.DeleteFile("SaveFile.es3", _settings);
        }
    }

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
        mainChar = GameObject.FindGameObjectWithTag("player");
        _settings = new ES3Settings("SaveGame.es3", ES3.EncryptionType.AES, "5454654");

        var isGameOver = EncryptedLoad<bool>("gameOver", "SaveGame.es3");

        if (isGameOver)
        {
            LoadGame("SaveRound.es3");
        }
        else
        {
            LoadGame();
        }
    }

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.StartDay:
                {
                    SaveGameData("SaveRound.es3");
                    break;
                }

            case EVENT_TYPE.StartNight:
                {
                    SaveGameData();
                    break;
                }


            case EVENT_TYPE.GameOver:
                {
                    EncryptedSaveValue("gameOver", true, "SaveGame.es3");
                    break;
                }

        }
    }

    void OnApplicationQuit()
    {
        if (GameStates.isDay)
        {
            SaveGameData();
        }

    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (GameStates.isDay)
            {
                SaveGameData();
            }
        }

    }
    private void LoadGame(string path = "SaveGame.es3")
    {
        InventoryItems = EncryptedLoad<Dictionary<int, SaveInfo>>("inventoryItemsDictionary", path, InventoryItems);
        mainChar.transform.position = EncryptedLoad<Vector3>("charPos", path, mainChar.transform.position);
        Map = EncryptedLoad<List<short>>("map", path, Map);
        GameStates.rating = EncryptedLoad<int>("rating", path);
        GameStates.BallLvl = EncryptedLoad<int>("ballLvl", path);
        GameStates.ArrowLvl = EncryptedLoad<int>("arrowLvl", path);
        GameStates.PickLvl = EncryptedLoad<int>("pickLvl", path);
        GameStates.Money = EncryptedLoad<int>("money", path, 5000);
        GameStates.Round = EncryptedLoad<int>("round", path);
        GameStates.GateCurrentHP = EncryptedLoad<int>("gateCurrentHp", path, 5000);

        if (path == "SaveGame.es3")
        {
            GameStates.Instance.CurrentTime = EncryptedLoad<int>("currentTime", path);
        }
    }

    private void EncryptedSaveValue<T>(string key, T data, string path)
    {
  
        ES3.Save(key, data, path, _settings);
    }

    private T EncryptedLoad<T>(string key, string path, T defaultValue = default )
    {
        if (ES3.FileExists(path, _settings) && ES3.KeyExists(key, _settings))
        {
            return (T)ES3.Load(key, path, _settings);
        }

        return defaultValue;
    }

    private void SaveGameData(string path = "SaveGame.es3")
    {
        EncryptedSaveValue("gameOver", false, "SaveGame.es3");

        EncryptedSaveValue("inventoryItemsDictionary", InventoryItems, path);
        EncryptedSaveValue("charPos", mainChar.transform.position, path);
        EncryptedSaveValue("map", Map, path);
        EncryptedSaveValue("rating", GameStates.rating, path);
        EncryptedSaveValue("ballLvl", GameStates.BallLvl, path);
        EncryptedSaveValue("arrowLvl", GameStates.ArrowLvl, path);
        EncryptedSaveValue("pickLvl", GameStates.PickLvl, path);
        EncryptedSaveValue("money", GameStates.Money, path);
        EncryptedSaveValue("round", GameStates.Round, path);
        EncryptedSaveValue("gateCurrentHp", GameStates.GateCurrentHP, path);

        if (path == "SaveGame.es3")
        {
            EncryptedSaveValue("currentTime", _timer.CurrentTime, path);
        }
    }

    public void ClearMap()
    {
        ES3Settings settings = new ES3Settings(ES3.EncryptionType.AES, "5454654");
        ES3.DeleteKey("map", "SaveFile.es3", settings);
    }

    public void GenerateBorders()
    {
        Debug.Log("StartGeneratingBorders");
        SceneGenerator sceneGenerator = FindObjectOfType<SceneGenerator>();
        Tilemap tilemap = GameObject.FindGameObjectWithTag("tileMap").GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[]  allTiles = tilemap.GetTilesBlock(bounds);
        Transform container = GameObject.FindGameObjectWithTag("borderContainer").transform;
        TileBase tile;
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                var index = x + y * bounds.size.x;

                tile = allTiles[index];

                if (tile != null)
                {
                    float spawnX = bounds.position.x + 0.5f + x;
                    float spawnY = bounds.position.y + 0.5f + y;
                    Vector2 newPos = new Vector2(spawnX, spawnY);
                    var tileNumberStr = tile.name.Split('_').Last();
                    var tileNumberInt = Int32.Parse(tileNumberStr);

                    if (tileNumberInt == 0)//static block in palette
                    {
                        Instantiate(sceneGenerator.element[29], newPos, Quaternion.identity, container);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveManager : MonoBehaviour
{
    private GameObject mainChar;
    [SerializeField]
    private List<short> map;
    public static Vector3 charPos;
    private ES3Settings _settings;
    public static Dictionary<int, int> InventoryItems { get; set; } = new Dictionary<int, int>();

    public List<short> Map
    {
        get => map;
        set => map = value;
    }

    private void Awake()
    {
        mainChar = GameObject.FindGameObjectWithTag("player");
        _settings = new ES3Settings(ES3.EncryptionType.AES, "5454654");
        if (ES3.KeyExists("inventoryItemsDictionary", _settings))
        {
            InventoryItems = (Dictionary<int, int>)ES3.Load("inventoryItemsDictionary", _settings);
        }
        if (ES3.KeyExists("charPos", _settings))
        {
            mainChar.transform.position = (Vector3)ES3.Load("charPos", _settings);
        }
        if (ES3.KeyExists("map", _settings))
        {
            Map = (List<short>)ES3.Load("map", _settings);
        }
        if (ES3.KeyExists("rating", _settings))
        {
            GameStates.Instance.rating = (int)ES3.Load("rating", _settings);
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveData();
    }

    private void EncryptedSaveValue<T>(string key, T data)
    {
        ES3.Save(key, data, _settings);
    }

    public void SaveData()
    {
        EncryptedSaveValue("inventoryItemsDictionary", InventoryItems);
        EncryptedSaveValue("charPos", mainChar.transform.position);
        EncryptedSaveValue("map", Map);
        EncryptedSaveValue("rating", GameStates.Instance.rating);  
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

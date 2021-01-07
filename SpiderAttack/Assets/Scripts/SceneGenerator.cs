using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using System.Collections.Generic;
using NGS.ExtendableSaveSystem;

public class SceneGenerator : MonoBehaviour, ISavableComponent
{
    public Transform sceneContainer;
    public GameObject[] element;
    //public Dictionary<int,int> saveList;
    public TestSaver testSaver;

    public Tilemap tilemap;

    private void Awake()
    {
        tilemap.gameObject.SetActive(false);
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        //TestSaver.saveList = new Dictionary<int,int>();
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                var index = x + y * bounds.size.x;
                //var val = testSaver.saveList[index];
                //if (val == 1)
                //{
                //    continue;
                //}
                TileBase tile = allTiles[index];

                if (tile != null)
                {
                    float spawnX = bounds.position.x + 0.5f + x;
                    float spawnY = bounds.position.y + 0.5f + y;
                    Vector2 newPos = new Vector2(spawnX, spawnY);
                    var tileNumberStr = tile.name.Split('_').Last();
                    var tileNumberInt = Int32.Parse(tileNumberStr);
                    //var count = Random.Range(0, element.Length);

                    if (element.ElementAtOrDefault(tileNumberInt) != null)
                    {
                         Instantiate(element[tileNumberInt], newPos, Quaternion.identity, sceneContainer);
                    }
                }
            }
        }

    }

    [SerializeField] private int _uniqueID;
    [SerializeField] private int _executionOrder;

    public int uniqueID
    {
        get
        {
            return _uniqueID;
        }
    }
    public int executionOrder
    {
        get
        {
            return _executionOrder;
        }
    }


    private void Reset()
    {
        _uniqueID = GetHashCode();
    }

    public ComponentData Serialize()
    {
        ExtendedComponentData data = new ExtendedComponentData();

        data.SetTransform("transform", transform);

        return data;
    }

    public void Deserialize(ComponentData data)
    {
        ExtendedComponentData unpacked = (ExtendedComponentData)data;

        unpacked.GetTransform("transform", transform);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class SceneGenerator : MonoBehaviour
{
    private TileBase tile;
    private BoundsInt bounds;
    private TileBase[] allTiles;

    public Transform sceneContainer;
    public GameObject[] element;
    public SaveManager saveManager;
    public Tilemap tilemap;

    private void Awake()
    {

        Debug.Log("StartGeneration");
        saveManager = FindObjectOfType<SaveManager>();
        //tilemap.gameObject.SetActive(false);
        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);

        //for (int x = 0; x < bounds.size.x; x++)
        //{
        //    for (int y = 0; y < bounds.size.y; y++)
        //    {
        //        var index = x + y * bounds.size.x;

        //        tile = allTiles[index];

        //        if (tile != null)
        //        {
        //            float spawnX = bounds.position.x + 0.5f + x;
        //            float spawnY = bounds.position.y + 0.5f + y;
        //            Vector2 newPos = new Vector2(spawnX, spawnY);
        //            var tileNumberStr = tile.name.Split('_').Last();
        //            var tileNumberInt = Int32.Parse(tileNumberStr);
        //            //var count = Random.Range(0, element.Length);

        //            if (element.ElementAtOrDefault(tileNumberInt) != null)
        //            {
        //                saveManager.map[index] = (short)(tileNumberInt * 10);
        //                //Instantiate(element[tileNumberInt], newPos, Quaternion.identity, sceneContainer);
        //            }
        //        }
        //    }
        //}
        GenerateMap();
        //GenerateBorders();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                var index = x + y * bounds.size.x;

                float spawnX = bounds.position.x + 0.5f + x;
                float spawnY = bounds.position.y + 0.5f + y;
                Vector2 newPos = new Vector2(spawnX, spawnY);
                //var tileNumberStr = tile.name.Split('_').Last();
                //var tileNumberInt = Int32.Parse(tileNumberStr);

                var mapIndex = saveManager.map[index] / 10;

                if (element.ElementAtOrDefault(mapIndex) == null)
                {
                    continue;


                }
                //saveManager.map[index] = (short)(tileNumberInt * 10);

                var gameObject = Instantiate(element[mapIndex], newPos, Quaternion.identity, sceneContainer);
                var indexx = saveManager.map[index];
                var crackIndex = GetRemainderOfDivision(indexx);
                if (crackIndex > 0)
                {
                    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.cracks[crackIndex - 1];
                    gameObject.GetComponent<BlockGroundDefault>().crackCount = (byte)crackIndex;
                }
            }
        }
    }

    private void GenerateBorders()
    {
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

                    if (element.ElementAtOrDefault(tileNumberInt) != null && tileNumberInt == 0)
                    {
                        Instantiate(element[0], newPos, Quaternion.identity, sceneContainer);
                    }
                }
            }
        }
    }

    private int GetRemainderOfDivision(int val)
    {
        if (val < 100)
        {
            return val % 10;
        }
        if (val < 1000)
        {
            return (val % 100) % 10;
        }
        return 0;
    }
}

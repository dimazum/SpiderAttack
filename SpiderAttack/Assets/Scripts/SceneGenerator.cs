using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

//[ExecuteInEditMode]
public class SceneGenerator : MonoBehaviour
{
    private TileBase tile;
    private BoundsInt bounds;
    private TileBase[] allTiles;

    public Transform sceneContainer;
    private GameObject borderContainer;

    [SerializeField]
    public GameObject[] element;
    public SaveManager saveManager;
    public Tilemap tilemap;

    public GameObject mainChar;


    public GameObject blackQuad;
    public GameObject greenQuad;
    public GameObject mainCharIcon;
    public Transform blackQuadContainer;

    private void Awake()
    {

        Debug.Log("StartGeneration");
        saveManager = FindObjectOfType<SaveManager>();
        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);
        Debug.Log(allTiles.Length);
        tilemap.gameObject.SetActive(false);
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
        GenerateMiniMap();
        //GenerateBorders();
    }

    public void CopyTilemapToMapList()
    {
        saveManager = FindObjectOfType<SaveManager>();
        saveManager.ClearMap();
        tilemap = GameObject.FindGameObjectWithTag("tileMap").GetComponent<Tilemap>();
        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);
        Debug.Log(allTiles.Length);

        for (var i = 0; i < saveManager.Map.Count; i++)
        {
            saveManager.Map[i] = 0;

        }

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                var index = x + y * bounds.size.x;

                tile = allTiles[index];

                if (tile != null)
                {
                    var tileNumberStr = tile.name.Split('_').Last();
                    var tileNumberInt = Int32.Parse(tileNumberStr);
                    if(tileNumberInt == -1)//if nothing
                    {
                        saveManager.Map[index] = (short)(tileNumberInt);
                        continue;
                    }
                    saveManager.Map[index] = (short)(tileNumberInt * 10);
                }
            }
        }
    }

    public void GenerateMiniMap()
    {
        for (int i = 0; i < blackQuadContainer.childCount; i++)
        {
            Destroy(blackQuadContainer.GetChild(i).gameObject);
        }

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                var index = x + y * bounds.size.x;

                float spawnX = bounds.position.x + 0.5f + x;
                float spawnY = bounds.position.y + 0.5f + y;
                var newPos = new Vector2(spawnX, spawnY);
                var mapIndex = saveManager.Map[index];

                if (mapIndex == -1 || mapIndex == 70) //empty or ladder
                {
                    var obj2 = Instantiate(blackQuad, blackQuadContainer);
                    obj2.transform.localPosition = newPos / 32;
                }

                if (mapIndex == -2)
                {
                    var obj2 = Instantiate(greenQuad, blackQuadContainer);
                    obj2.transform.localPosition = newPos / 32;
                }
            }
        }

        //var mainCharPos = tilemap.WorldToCell(mainChar.transform.position) / 32;
        var mainCharIconObj = Instantiate(mainCharIcon, blackQuadContainer);
        mainCharIconObj.transform.localPosition = mainChar.transform.position / 32;



        //var index2 = mainCharPos.x - bounds.x + (mainCharPos.y - bounds.y) * bounds.size.x;
        //saveManager.Map[index2] = ;
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
                var mapIndex = saveManager.Map[index]/10;


                if (element.ElementAtOrDefault(mapIndex) == null)
                {
                    continue;
                }

                var gameObject = Instantiate(element[mapIndex], newPos, Quaternion.identity, sceneContainer);
                var mapValue = saveManager.Map[index];
                var crackIndex = GetRemainderOfDivision(mapValue);
                if (crackIndex > 0)
                {
                    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.cracks[crackIndex - 1];
                    gameObject.GetComponent<IHitableBlock>().CrackCount = (byte)crackIndex;
                }
            }
        }
    }

    public void RemoveBorders()
    {
        Debug.Log("StartRemovingBorders");
        Transform container = GameObject.FindGameObjectWithTag("borderContainer").transform;
        for (int i = 0; i < container.transform.childCount; i++)
        {
            DestroyImmediate(container.transform.GetChild(i).gameObject);
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

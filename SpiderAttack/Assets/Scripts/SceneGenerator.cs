using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class SceneGenerator : MonoBehaviour
{
    public Transform sceneContainer;
    public GameObject[] element;

    public Tilemap tilemap;

    private void Awake()
    {
        tilemap.gameObject.SetActive(false);
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

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
}

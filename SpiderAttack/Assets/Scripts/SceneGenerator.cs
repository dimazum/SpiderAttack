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

    private float startX_CSV = -150;    //стартовая позиция по икс
    private float startY_CSV = 3.5f;    //стартовая позиция по игрек

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


    void Start()
    {
        //BuildScene();
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void BuildScene()
    {

        string dbPath = "";

            dbPath = Application.streamingAssetsPath + "/" + 1 + ".csv";

        if (!File.Exists(dbPath))
        {
            Debug.Log("Не могу найти файл! (" + dbPath + ")");

            return;
        }

        //читаем файл в массив строк
        string[] AllText = File.ReadAllLines(dbPath);

        for (int y = 0; y < AllText.Length; y++)
        {
            string[] stroka = AllText[y].Split(new char[] { ',' });

            for (int x = 0; x < stroka.Length; x++)
            {
                float spawnX = startX_CSV + x;
                float spawnY = startY_CSV - y;
                Vector2 newPos = new Vector2(spawnX, spawnY);

                int el;
                if (int.TryParse(stroka[x], out el))
                    if (el >= 0)
                    {
                        var count = UnityEngine.Random.Range(0, element.Length);
                        //ставим фон
                        //Instantiate(element[26], newPos, Quaternion.identity);
                        //ставим элемент
                        Instantiate(element[count], newPos, Quaternion.identity, sceneContainer);
                        //Debug.Log(el);
                    }
            }
        }

    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{
    public Transform sceneContainer;
    public GameObject[] element;

    private float startX_CSV = -150;    //стартовая позиция по икс
    private float startY_CSV = 3.5f;    //стартовая позиция по игрек

    void Start()
    {
        BuildScene();
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void BuildScene()
    {
        //var variableForPrefab = (GameObject)Resources.Load("prefabs/blockGroundTest", typeof(GameObject));
        //var variableForPrefab = (GameObject)Resources.Load("prefabs/blockGroundTest2", typeof(GameObject));
        //var blocksContainer = GameObject.Find("BlockContainer");


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
                        var count = Random.Range(0, element.Length);
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

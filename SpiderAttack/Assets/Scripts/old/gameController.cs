using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public float testHP;


    public Text txtMoney; //ссылка на текст с деньгами
    public GameObject mapSlider;//ссылка на слайдер карты
    public Canvas canvas;//ссылка на канвас для анимации
    public GameObject[] element; //массив с элементами сцены
    public cameraController cC;

    public float startX_CSV;    //стартовая позиция по икс
    public float startY_CSV;    //стартовая позиция по игрек


    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void BuildScene()
    {



        string dbPath = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "1.csv");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "/1";
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            dbPath = realPath;
        }
        else
        {
            dbPath = Application.streamingAssetsPath + "/1.csv";




        }



        startX_CSV = -150;
        startY_CSV = 3;






        if (!File.Exists(dbPath))
        {
            Debug.Log("Не могу найти файл! (" + dbPath + ")");

            return;
        }
        //читаем файл в массив строк
        string[] AllText = File.ReadAllLines(dbPath);

        for (int y = 0; y < AllText.Length /5; y++)
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
                        //ставим фон
                        //Instantiate(element[26], newPos, Quaternion.identity);
                        //ставим элемент
                        Instantiate(element[el], newPos, Quaternion.identity);
                        //Debug.Log(el);
                    }
            }
        }

    }

    public void BuildSceneFon()
    {



        string dbPath = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "2.csv");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "/2";
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            dbPath = realPath;
        }
        else
        {
            dbPath = Application.streamingAssetsPath + "/2.csv";




        }



        startX_CSV = -150;
        startY_CSV = 3;






        if (!File.Exists(dbPath))
        {
            Debug.Log("Не могу найти файл! (" + dbPath + ")");

            return;
        }
        //читаем файл в массив строк
        string[] AllText = File.ReadAllLines(dbPath);

        for (int y = 0; y < AllText.Length/5  ; y++)
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
                        //ставим фон
                        Instantiate(element[26], newPos, Quaternion.identity);
                        //ставим элемент
                        //Instantiate(element[el], newPos, Quaternion.identity);
                        //Debug.Log(el);
                    }
            }
        }

    }

    void Start()
    {
        BuildScene();
        BuildSceneFon();

    }

    void Update()
    {
        //управление картой
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (gameState.isInMap)
            {
                mapSlider.SetActive(false);
                canvas.GetComponent<Animator>().Play("mapSliderOut");
                // Camera.main.GetComponent<Animator>().StopPlayback();
                gameState.isInMap = false;
                cC.animator.Play("mapOut");
                //Camera.main.orthographicSize = 2.5f;
                //Debug.Log("Вышел из карту!");
            }
            else
            {
                mapSlider.SetActive(true);
                canvas.GetComponent<Animator>().Play("mapSliderIn");
                //Camera.main.GetComponent<Animator>().StopPlayback();
                gameState.isInMap = true;
                cC.animator.Play("inMap");
                //Camera.main.orthographicSize = 8.0f;

                mapSlider.GetComponent<Slider>().value = 0f;
                //Debug.Log("Вошел в карту!");
            }
        }
    }

    void FixedUpdate()
    {
        txtMoney.text = gameState.currentPlayer.money + "$";
    }

    public void OnChangedMapSlider()
    {
        Camera.main.orthographicSize = 8f + mapSlider.GetComponent<Slider>().value * 16f;
    }



}

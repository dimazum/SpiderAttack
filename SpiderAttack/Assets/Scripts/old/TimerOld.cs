using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerOld : MonoBehaviour {
    public GameObject Skelet;
    public Transform spownBox;
    public float sec = 0;
    public float sec2 = 0;
    public int min = 4;
    public int hour = 0;
    public int day = 0;
    public GameObject enemy;
    public Text TextTime;
   
    public int alpha;

	// Use this for initialization
	void Start () {
        min = 4;

    }

    // Update is called once per frame
    void Update()
    {
        //TextTime.text= ("День: " + day + "  Время: " + hour + ":" + min + ":" + Mathf.Round(sec));
        TextTime.text = ("До ночи: " +  min + ":" + Mathf.Round(sec));
        sec -= Time.deltaTime;
        sec2 += Time.deltaTime;
        if (sec <= 0)
        {
            min--;
            sec = 60;
        }
        if (min >= 60)
        {
            hour++;
            min = 0;
        }
        if (hour >= 24)
        {
            day++;
            hour = 0;
        }
        enemy = GameObject.FindWithTag("Krab");
        //if (sec2 >= 25)
        if (enemy == null)
        {
            //Instantiate(Skelet, spownBox.position, Quaternion.identity);
            sec2 = 0;
        }
    }
    //private void OnGUI()
    //{
    //    GUI.Box(new Rect(5, 5, 200, 20), "День: " + day + "  Время: " + hour + ":" + min+":"+ Mathf.Round( sec));
    //}
   
}

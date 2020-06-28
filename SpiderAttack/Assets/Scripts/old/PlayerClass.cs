using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerClass : ScriptableObject {

    public string Name;

    //инвентарь игрока
    public List<InventoryItem> inventory = new List<InventoryItem>();

    //инвентарь игрока
    public Dictionary<string, int> shopDictionary = new Dictionary<string, int>();

    //сумка игрока с ресурсами
    //public List<itemBag> playerBag = new List<itemBag>();

    //сумка игрока с реусурсами
    public Dictionary<string, int> bagDictionary = new Dictionary<string, int>();

    //колличество денег игрока
    public int money;

    private int health;

    public const int MAXHEALTH = 100;
    public const int MINHEALTH = 0;

    public int Health
    {
        set
        {
            if (value < MINHEALTH)
                health = MINHEALTH;
            else 
            if (value > MAXHEALTH)
                health = MAXHEALTH;
            else
                health = value;
        }
        get
        {
            return health;
        }
    }

    public PlayerClass()
    {
        health = 100;
        money = 600;
        this.Name = "Miner";

    }
}

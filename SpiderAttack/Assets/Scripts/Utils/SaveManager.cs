using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private GameObject mainChar;
    public static Dictionary<int, int> InventoryItems { get; set; } = new Dictionary<int, int>();
    public List<short> map;
    public static Vector3 charPos;
    private ES3Settings settings;


    private void Awake()
    {
        mainChar = GameObject.FindGameObjectWithTag("player");
        settings = new ES3Settings(ES3.EncryptionType.AES, "5454654");
        if (ES3.KeyExists("inventoryItemsDictionary", settings))
        {
            InventoryItems = (Dictionary<int, int>)ES3.Load("inventoryItemsDictionary", settings);
        }
        if (ES3.KeyExists("charPos", settings))
        {
            mainChar.transform.position = (Vector3)ES3.Load("charPos", settings);
        }
        if (ES3.KeyExists("map", settings))
        {
            map = (List<short>)ES3.Load("map", settings);
        }
    }

    void OnApplicationQuit()
    {
        EncryptedSaveValue("inventoryItemsDictionary", InventoryItems);
        EncryptedSaveValue("charPos", mainChar.transform.position);
        EncryptedSaveValue("map", map);
    }

    private void OnApplicationPause(bool pause)
    {
        EncryptedSaveValue("inventoryItemsDictionary", InventoryItems);
        EncryptedSaveValue("charPos", mainChar.transform.position);
        EncryptedSaveValue("map", map);
    }

    private void EncryptedSaveValue<T>(string key, T data)
    {
        ES3.Save(key, data, settings);
    }
}

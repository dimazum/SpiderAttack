using NGS.ExtendableSaveSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InventoryData : MonoBehaviour, IListener
{
    public TypeCollection[] collections;
    public Tilemap tilemap;
    BoundsInt bounds;
    public TestSaver testSaver;


    void Start()
    {
        bounds = tilemap.cellBounds;
        EventManager.Instance.AddListener(EVENT_TYPE.GetResurs, this);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("REFRESH");
        }
        
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GetResurs:
                if (Param == null)
                {
                    return;
                }
                var resursInfo = (ResursInfo)Param;
                collections[(int)resursInfo.ItemGroup].itemTypes[(int)resursInfo.MineralCategory].Qty++;


                var tilePos = tilemap.WorldToCell(resursInfo.Position);
                Debug.Log("location:" + tilePos);
                tilemap.SetTile(tilePos, null);
                tilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y, tilePos.z), null);
                bounds = tilemap.cellBounds;
                var test = tilePos.x -bounds.x + ( tilePos.y-bounds.y) * bounds.size.x;
                //var test = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
                testSaver.saveList[test]= 1;
                break;
        }
    }
}



//снаряды: стрелы, бомбы
//ресурсы:20шт
//рецепты
//свитки телепортации, фонари, лестницы, бомбы

//бомбы:
//1.Бросаешь  и 5 секунд лежит, потом взпывается
// супер бомба
// 0.static stone
//1. stone
//2. ground1 coal amber copper gold
//3. ground2 benetiod opal radolit ore 
//4. ground3 pomegranate emerald  agat demantoit 
//5. ground4 daimont ametist malachite topaz
//6. ground5 paraiba ruby taaffetite chrysoberil
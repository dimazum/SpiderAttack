using Assets.Scripts.Utils.enums;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemsData2 : MonoBehaviour, IListener
{
    public TypeCollection[] collections2;
    public Tilemap tilemap;
    BoundsInt bounds;
    private SaveManager saveManager;

    void Start()
    {
        bounds = tilemap.cellBounds;
        EventManager.Instance.AddListener(EVENT_TYPE.GetResurs, this);
        EventManager.Instance.AddListener(EVENT_TYPE.HitResurs, this);
        saveManager = FindObjectOfType<SaveManager>();

    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GetResurs:
                {
                    if (Param == null)
                    {
                        return;
                    }
                    var resursInfo = (ResursInfo)Param;
                    var item = collections2[(int)resursInfo.ItemGroup].itemTypes[(int)resursInfo.MineralCategory];
                    if (!item.endlesQty)
                    {
                        item.Qty++;
                    }
                    var tilePos = tilemap.WorldToCell(resursInfo.Position);
                    var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
                    saveManager.map[index] = 0;
                }
                break;

            case EVENT_TYPE.HitResurs:
                {
                    if (Param == null)
                    {
                        return;
                    }
                    var resursInfo = (ResursInfo)Param;

                    var item = collections2[(int)resursInfo.ItemGroup].itemTypes[(int)resursInfo.MineralCategory] as ISavableOnMap;
                    var mapIndex = item.MapIndex;
                    var tilePos = tilemap.WorldToCell(resursInfo.Position);
                    var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
                    saveManager.map[index] = (short)(mapIndex + resursInfo.CrackCount);
                }
                break;
        }
    }
}


[System.Serializable]
public class TypeCollection
{
    public ItemGroup ItemGroup;
    public BaseItemType[] itemTypes;
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



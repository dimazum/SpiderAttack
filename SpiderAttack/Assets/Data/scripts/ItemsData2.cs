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
                    var resourceBlockInfo = (ResourceBlockInfo)Param;

                    foreach (var resourceInfo in resourceBlockInfo.ResourceInfos)
                    {
                        var item = collections2[resourceInfo.ItemGroup].itemTypes[resourceInfo.Category];
                        if (!item.endlesQty)
                        {
                            item.Qty+= resourceInfo.Qty;
                        }
                    }

                   
                    var tilePos = tilemap.WorldToCell(resourceBlockInfo.Position);
                    //var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
                    //saveManager.Map[index] = -1;
                    SaveHelper.Instance.DeleteObjecFromPosition(tilePos);
                }
                break;

            case EVENT_TYPE.HitResurs:
                {
                    if (Param == null)
                    {
                        return;
                    }
                    var resourceBlockInfo = (ResourceBlockInfo)Param;
                    short mapIndex = 0;
                    if (resourceBlockInfo.ResourceInfos.Count == 1)//single resource
                    {
                        var item = collections2[resourceBlockInfo.ResourceInfos[0].ItemGroup].itemTypes[resourceBlockInfo.ResourceInfos[0].Category] as ISavableOnMap; 
                        mapIndex = item.MapIndex;
                    }

                    if (resourceBlockInfo.ResourceInfos.Count > 1)//multi resource
                    {
                        mapIndex = resourceBlockInfo.ResourceInfos[0].MapIndex;
                    }
                   
                    var tilePos = tilemap.WorldToCell(resourceBlockInfo.Position);
                    var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
                    saveManager.Map[index] = (short)(mapIndex + resourceBlockInfo.CrackCount);
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




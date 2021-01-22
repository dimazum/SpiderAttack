using Assets.Scripts.Utils.enums;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveHelper : Singleton<SaveHelper>
{
    [SerializeField]
    private ItemsData2 itemData;
    [SerializeField]
    private Tilemap tilemap;
    private BoundsInt bounds;
    private SaveManager saveManager;

    void Start()
    {
        bounds = tilemap.cellBounds;
        saveManager = FindObjectOfType<SaveManager>();
    }

    public void PutObjecPosition(Vector3 position, ItemGroup itemGroup, int category = 0)
    {
        var item = itemData.collections2[(int)itemGroup].itemTypes[category] as ISavableOnMap;
        var mapIndex = item.MapIndex;
        var tilePos = tilemap.WorldToCell(position);
        var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
        saveManager.Map[index] = mapIndex;
    }

    public void DeleteObjecFromPosition(Vector3 position)
    {
        var tilePos = tilemap.WorldToCell(position);
        var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
        saveManager.Map[index] = -1;
    }

    public void PutStaticBlockPosition(Vector3 position)
    {
        var tilePos = tilemap.WorldToCell(position);
        var index = tilePos.x - bounds.x + (tilePos.y - bounds.y) * bounds.size.x;
        saveManager.Map[index] = -2;
    }
}

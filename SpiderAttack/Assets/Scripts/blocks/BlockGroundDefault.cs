using Assets.Scripts.enums;
using Assets.Scripts.Utils.enums;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroundDefault : BaseGroundBlock
{

    public GroundCategory groundCategory;

    protected override List<ResourceInfo> GetResourceInfos()
    {
        return new List<ResourceInfo>()
        {
            new ResourceInfo
            {
                ItemGroup = (int) ItemGroup.Grounds,
                Category = (int) groundCategory,
                Qty = 1
            }
        };
    }
}

public class ResourceBlockInfo
{
    public List<ResourceInfo> ResourceInfos { get; set; }
    public Vector3 Position { get; set; }
    public short CrackCount { get; set; }
}

[System.Serializable]
public class ResourceInfo
{
    [SerializeField]
    private ItemGroup _itemGroup;

    [SerializeField]
    private int _category;

    [SerializeField]
    private int _qty;

    [SerializeField]
    private short _mapIndex;

    public  int ItemGroup
    {
        get => (int)_itemGroup;
        set => _itemGroup = (ItemGroup)value;
    }

    public  int Category
    {
        get => _category;
        set => _category = value;
    }

    public  int Qty
    {
        get => _qty;
        set => _qty = value;
    }

    public short MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}


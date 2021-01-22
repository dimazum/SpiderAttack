using Assets.Scripts.Utils.enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrowResurs : BaseGroundBlock
{
    [SerializeField]
    ItemGroup itemGroup;
    [SerializeField]
    ArrowCategory arrowCategory;


    protected override List<ResourceInfo> GetResourceInfos()
    {
        return new List<ResourceInfo>()
            {new ResourceInfo {ItemGroup = (int) itemGroup, Category = (int) arrowCategory, Qty = 1}};
    }
}

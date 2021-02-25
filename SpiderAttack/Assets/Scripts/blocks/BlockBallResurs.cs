using Assets.Scripts.Utils.enums;
using System.Collections.Generic;
using UnityEngine;

public class BlockBallResurs : BaseGroundBlock
{
    [SerializeField]
    ItemGroup itemGroup;
    [SerializeField]
    BallCategory arrowCategory;


    protected override List<ResourceInfo> GetResourceInfos()
    {
        return new List<ResourceInfo>()
            {new ResourceInfo {ItemGroup = (int) itemGroup, Category = (int) arrowCategory, Qty = 1}};
    }
}

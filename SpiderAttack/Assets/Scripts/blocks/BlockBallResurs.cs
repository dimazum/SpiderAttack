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
        throw new System.NotImplementedException();
    }
}

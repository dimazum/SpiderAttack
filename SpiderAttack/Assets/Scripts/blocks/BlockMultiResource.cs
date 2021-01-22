using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils.enums;
using UnityEngine;

public class BlockMultiResource : BaseGroundBlock
{

    public List<ResourceInfo> resources;


    protected override List<ResourceInfo> GetResourceInfos()
    {
        return resources;
    }
    
}
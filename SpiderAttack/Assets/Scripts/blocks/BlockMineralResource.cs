using System.Collections.Generic;
using Assets.Scripts.Utils.enums;

public class BlockMineralResource : BaseGroundBlock
{
    public MineralCategory mineralCategory;

    protected override List<ResourceInfo> GetResourceInfos()
    {
        return new List<ResourceInfo>()
        {
            new ResourceInfo
            {
                ItemGroup = (int) ItemGroup.Minerals,
                Category = (int) mineralCategory,
                Qty = 1
            }
        };
    }
}

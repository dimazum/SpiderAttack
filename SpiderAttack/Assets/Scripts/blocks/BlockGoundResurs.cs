using Assets.Scripts.Utils.enums;
using UnityEngine;

public class BlockGoundResurs : BlockGroundDefault
{

}

public struct ResursInfo
{
    public ItemGroup ItemGroup { get; set; }
    public MineralCategory MineralCategory { get; set; }
    public Vector3 Position { get; set; }
    public short CrackCount { get; set; }
}

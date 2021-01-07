using Assets.Scripts.Utils.enums;
using UnityEngine;

public class BlockGoundResurs : BlockGroundDefault
{
    public ItemGroup itemGroup;
    public MineralCategory mineralCategory;


    public override void Hit()
    {
        if (crackCount < 4)
        {
            crackCount++;
        }

        if (crackCount == 4)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.GetResurs, this, new ResursInfo() { ItemGroup = itemGroup, MineralCategory = mineralCategory, Position = gameObject.transform.position  });
            SetDeadState();
            CheckMoveDownObject();
            return;
        }

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.cracks[crackCount - 1];
    }
}

public struct ResursInfo
{
    public ItemGroup ItemGroup { get; set; }
    public MineralCategory MineralCategory { get; set; }

    public Vector3 Position { get; set; }
}

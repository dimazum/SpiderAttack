using Assets.Scripts.enums;
using Assets.Scripts.Utils.enums;
using UnityEngine;

public class BlockGroundDefault : MonoBehaviour
{
    public byte crackCount;
    private float hitRange = 1f;


    public virtual void Hit()
    {
        if (crackCount < 4)
        {
            crackCount++;
        }

        if (crackCount == 4)
        {
            SetDeadState();
            CheckMoveDownObject();
            return;
        }

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.cracks[crackCount - 1];
    }

    protected void SetDeadState()
    {
        gameObject.layer = Layer.Dead;
        gameObject.GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.blockBackground[0];//set background instead of ground
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;//remove cracks

    }

    protected void CheckMoveDownObject()
    {
        var hitTop = Physics2D.Raycast(transform.position, Vector2.up, hitRange, 1 << Layer.Ladders|1<<Layer.Stones );
        if (hitTop.collider != null)
        {
            var moveDownObj = hitTop.collider.gameObject.GetComponent<IMoveDown>();
            moveDownObj?.MoveObjectDown();
        }
    }
  
}
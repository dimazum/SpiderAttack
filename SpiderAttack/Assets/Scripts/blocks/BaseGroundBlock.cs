using Assets.Scripts.enums;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGroundBlock : MonoBehaviour, ICheckFallingObj, IHitableBlock
{
    [SerializeField] private byte _minPickLvl;
    [SerializeField] private bool _isGround;

    public bool IsGround
    {
        get => _isGround;
        set => _isGround = value;
    }

    public byte MinPickLvl
    {
        get => _minPickLvl;
        set => _minPickLvl = value;
    }
    public byte CrackCount { get; set; }
    

    public void Hit()
    {
        if (CrackCount < 4)
        {
            CrackCount++;
        }

        if (CrackCount == 4)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.GetResurs, this, CreateResourceBlockInfo());
            SetDeadState();
            CheckMoveDownObject();
            return;
        }
        EventManager.Instance.PostNotification(EVENT_TYPE.HitResurs, this, CreateResourceBlockInfo());
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.cracks[CrackCount - 1];
    }


    protected abstract List<ResourceInfo> GetResourceInfos();
   
    private ResourceBlockInfo CreateResourceBlockInfo()
        =>
            new ResourceBlockInfo()
            {
                ResourceInfos = GetResourceInfos(),
                Position = gameObject.transform.position,
                CrackCount = CrackCount
            };


    protected void SetDeadState()
    {
        gameObject.layer = Layer.Dead;
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;//remove cracks
        if (transform.childCount > 1)
        {
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = null;//remove icon
        }
    }

    public void CheckMoveDownObject()
    {
        var hitTop = Physics2D.Raycast(transform.position, Vector2.up, 1f, 1 << Layer.Ladders | 1 << Layer.Stones);
        if (hitTop.collider != null)
        {
            var moveDownObj = hitTop.collider.gameObject.GetComponent<IMoveDown>();
            moveDownObj?.MoveObjectDown();
        }
    }
}
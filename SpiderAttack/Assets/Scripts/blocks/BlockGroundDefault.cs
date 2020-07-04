using Assets.Scripts.enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockGroundDefault : MonoBehaviour
{

    public delegate void AccountHandler(string message);
    public event AccountHandler Notify;


    public byte crackCount;
    private Sprite[] sprites;
    private Sprite downSpike;
    private Sprite topSpike;
    private Sprite doubleSpike;

    private byte blockLayer = 10;
    private byte deadLayer = 8;
    private float hitRange = 1f;

    private byte _crackCount;


    public void Hit()
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

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.cracks[crackCount-1];
    }

    private void SetDeadState()
    {
        Notify?.Invoke("hi");

        gameObject.layer = deadLayer;
        gameObject.GetComponent<SpriteRenderer>().sprite = SpriteData.Instance.blockBackground[0];//set background instead of ground
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;//remove cracks

    }

    //SetSpikesToBlock();
    private void CheckMoveDownObject()
    {
        var hitTop = Physics2D.Raycast(transform.position, Vector2.up, hitRange, 1 << Layer.Ladders);
        if (hitTop.collider != null)
        {
            //var ladderhitTop.collider.GetComponent<LadderController>();
            var test22 = hitTop.collider.gameObject.GetComponent<LadderController>();
            test22.MoveObjectDown(0);
        }
    }

    private void SetSpikesToBlock()
    {
        if (sprites == null || sprites.Length == 0)
        {
            sprites = Resources.LoadAll<Sprite>("prefabs/sprites/tm_tile");
            downSpike = sprites[10];
            topSpike = sprites[11];
            doubleSpike = sprites[12];
        }

        var hitDown = Physics2D.Raycast(transform.position, Vector2.down, hitRange, 1 << blockLayer | 1 << deadLayer);
        var hitTop = Physics2D.Raycast(transform.position, Vector2.up, hitRange, 1 << blockLayer | 1 << deadLayer);

        int? hitDownLayer = hitDown.collider?.gameObject.layer;
        int? hitTopLayer = hitTop.collider?.gameObject.layer;

        //down - not dead    top - not dead
        if (hitDownLayer == blockLayer && hitTopLayer == blockLayer)
        {
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = doubleSpike;
        }

        //down - dead    top - not dead
        else if (hitDownLayer == blockLayer && hitTopLayer != blockLayer)
        {
            SetNextSpikes(hitTop.collider?.gameObject, Vector2.up, topSpike);
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = downSpike;
        }

        //down - not dead  top - dead
        else if (hitDownLayer != blockLayer && hitTopLayer == blockLayer)
        {
            SetNextSpikes(hitTop.collider?.gameObject, Vector2.down, downSpike);
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = topSpike;
        }

        //down - dead   top - dead
        else if (hitDownLayer != blockLayer && hitTopLayer != blockLayer)
        {
            SetNextSpikes(hitDown.collider?.gameObject, Vector2.down, downSpike);
            SetNextSpikes(hitTop.collider?.gameObject, Vector2.up, topSpike);
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void SetNextSpikes(GameObject gameObject, Vector2 direction, Sprite sprite)
    {
        if (gameObject != null)
        {
            var hitTop = Physics2D.Raycast(gameObject.transform.position, direction, hitRange, 1 << blockLayer | 1 << deadLayer);

            if (hitTop.collider?.gameObject.layer != blockLayer)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;

                if (hitTop.collider != null)
                {
                    SetNextSpikes(hitTop.collider.gameObject, direction, sprite);
                }
            }
            else if (hitTop.collider?.gameObject.layer == blockLayer)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
    }
}
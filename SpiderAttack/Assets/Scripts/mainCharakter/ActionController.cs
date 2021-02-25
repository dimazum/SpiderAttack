using Assets.Scripts.enums;
using Assets.Scripts.Utils.enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    private InventoryController _inventoryController;
    [SerializeField]
    private GameObject ladder;
    [SerializeField]
    private GameObject _torch;
    [SerializeField]
    private GameObject bomb;
    private Transform sceneContainer;
    [SerializeField]
    private Transform _camera;
    private float teleportTime = 2.5f;
    private Coroutine teleportCo;
    private const int LayerMaskDefault = 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones | 1 << Layer.Torch;

    private bool isTeleporting;

    private void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
    }

    public void SetLadder()
    {
        float posX;
        if (transform.position.x > 0)
        {
            posX = (int)transform.position.x + 0.5f;
        }
        else
        {
            posX = (int)transform.position.x - 0.5f;
        }

        float posY = Mathf.Round(transform.position.y) + 0.5f;
        Vector2 pos = new Vector2(posX, posY);

        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << Layer.Ladders | 1<< Layer.Torch);

        if (check == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
        {
            if (Physics2D.Raycast(pos, Vector2.down, 1f, LayerMaskDefault).collider != null)//стреляем вниз
            {
                Instantiate(ladder, pos, Quaternion.identity, sceneContainer); //если ничего не нашли внизу не пусто, то ставим лестницу
                EventManager.Instance.PostNotification(EVENT_TYPE.SetLadder, this);
                SaveHelper.Instance.PutObjecPosition(pos, ItemGroup.Ladders);
                _inventoryController.UseItem();
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        }
    }

    public void SetTorch()
    {
        float posX;
        if (transform.position.x > 0)
        {
            posX = (int)transform.position.x + 0.5f;
        }
        else
        {
            posX = (int)transform.position.x - 0.5f;
        }

        float posY = Mathf.Round(transform.position.y) + 0.5f;
        Vector2 pos = new Vector2(posX, posY);

        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << Layer.Ladders | 1 << Layer.Torch);

        if (check == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
        {
            
            if (Physics2D.Raycast(pos, Vector2.down, 1f, LayerMaskDefault).collider != null)//стреляем вниз
            {
                Instantiate(_torch, pos, Quaternion.identity, sceneContainer); //если ничего не нашли внизу не пусто, то ставим лестницу
                //EventManager.Instance.PostNotification(EVENT_TYPE.SetLadder, this);
                SaveHelper.Instance.PutObjecPosition(pos, ItemGroup.Torchs);
                _inventoryController.UseItem();
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        }
    }

    public void StartTeleport()
    {
        if (isTeleporting) return;
        EventManager.Instance.PostNotification(EVENT_TYPE.StartTeleport, this, teleportTime);
        teleportCo =  StartCoroutine(CoStartTeleport());
        _inventoryController.UseItem();
    }

    public void StopTeleport()
    {
        isTeleporting = false;
        if (teleportCo == null) return;
        StopCoroutine(teleportCo);
        EventManager.Instance.PostNotification(EVENT_TYPE.FinishTeleport, this);
    }


    public void SetBomb()
    {
        float posX;
        int index = transform.position.x > 0? 1:-1;
        if(transform.localScale.x < 0)
        {
            posX =  (int)transform.position.x + (index * 0.5f) + 0.3f;
        }
        else
        {
            posX =  (int)transform.position.x + (index* 0.5f) - 0.3f;
        }

        float posY = Mathf.Round(transform.position.y) + 0.3f;
        Vector2 pos = new Vector2(posX, posY);

        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << Layer.Ladders);


        if (check == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
        {
            if (Physics2D.Raycast(pos, Vector2.down, 1f, LayerMaskDefault).collider != null)//стреляем вниз
            {
                var bombObj = Instantiate(bomb, pos, Quaternion.identity, sceneContainer);

                Vector2 explosionDir = new Vector2(transform.localScale.x, 0);
                StartCoroutine(BombDestroy(pos, bombObj, explosionDir));
                _inventoryController.UseItem();
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        } 
    }

    private IEnumerator CoStartTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(teleportTime);
        _camera.SetParent(gameObject.transform);
        transform.position = new Vector3(-18, 0.36f, 0);
        _camera.SetParent(null);
        EventManager.Instance.PostNotification(EVENT_TYPE.FinishTeleport, this);
        isTeleporting = false;
    }

    private IEnumerator BombDestroy( Vector2 mousePos, GameObject bomb, Vector2 explosionDir)
    {
        yield return new WaitForSeconds(2.7f);
        RightBombWave(mousePos,  explosionDir);
        LeftBombWave(mousePos,  explosionDir);
        CenterBombWave(mousePos);

        Destroy(bomb);
    }

    private void RightBombWave(Vector2 mousePos, Vector2 explosionDir)
    {
        var block = Physics2D.Raycast(mousePos, -explosionDir, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones | 1 << Layer.Player | 1 << Layer.Torch).collider?.gameObject;

        if (block == null) return;
        if (block.CompareTag("player"))
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.DynamiteHurtChar, this);
            return;
        }

        if (TryGetComponent<ICheckFallingObj>(out var component))
        {
            component.CheckMoveDownObject();
        }

        SaveHelper.Instance.DeleteObjecFromPosition(block.transform.position);
        Destroy(block);
    }

    private void CenterBombWave(Vector2 mousePos)
    {
        var block = Physics2D.OverlapPoint(mousePos, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones | 1 << Layer.Player | 1 << Layer.Torch)?.gameObject;
        if (block == null) return;

        if (block.CompareTag("player"))
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.DynamiteHurtChar, this);
            return;
        }

        block.GetComponent<ICheckFallingObj>().CheckMoveDownObject();
        SaveHelper.Instance.DeleteObjecFromPosition(block.transform.position);
        Destroy(block);
 
    }

    private void LeftBombWave(Vector2 mousePos, Vector2 explosionDir)
    {
        var block = Physics2D.Raycast(mousePos, explosionDir, .8f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones | 1 << Layer.Player | 1 << Layer.Torch).collider?.gameObject;
        if (block == null) return;
        if (!block.CompareTag("player")) return;

        EventManager.Instance.PostNotification(EVENT_TYPE.DynamiteHurtChar, this);
    }
}

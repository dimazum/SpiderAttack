using Assets.Scripts.enums;
using Assets.Scripts.Utils.enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public GameObject ladder;
    public GameObject bomb;
    public Transform sceneContainer;
    public Transform camera;
    private float teleportTime = 2.5f;

    //public Slider Slider;
    Coroutine teleportCo;

    void Start()
    {
        
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

        Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << Layer.Ladders);

        if (check == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
        {
            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones).collider != null)//стреляем вниз
            {
                Instantiate(ladder, pos, Quaternion.identity, sceneContainer); //если ничего не нашли внизу не пусто, то ставим лестницу
                EventManager.Instance.PostNotification(EVENT_TYPE.SetLadder, this);
                SaveHelper.Instance.PutObjecPosition(pos, ItemGroup.Ladders);
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        }
    }

    public void StartTeleport()
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.StartTeleport, this, teleportTime);
        teleportCo =  StartCoroutine(CoStartTeleport());
    }

    public void StopTeleport()
    {
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
            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones).collider != null)//стреляем вниз
            {
                var bombObj = Instantiate(bomb, pos, Quaternion.identity, sceneContainer);

                //EventManager.Instance.PostNotification(EVENT_TYPE.SetLadder, this);
                Vector2 explosionDir = new Vector2(transform.localScale.x, 0);
                StartCoroutine(BombDestroy(pos, bombObj, explosionDir));
            }
        }
        else
        {
            Debug.Log("Место занято, не могу поставить лестницу! Стоит: ");
        }

        
    }

    private IEnumerator CoStartTeleport()
    {
        yield return new WaitForSeconds(teleportTime);
        camera.SetParent(gameObject.transform);
        transform.position = new Vector3(-18, 0, 0);
        camera.SetParent(null);
        EventManager.Instance.PostNotification(EVENT_TYPE.FinishTeleport, this);
    }

    private IEnumerator BombDestroy( Vector2 mousePos, GameObject bomb, Vector2 explosionDir)
    {
        yield return new WaitForSeconds(2.7f);
        GameObject block = Physics2D.Raycast(mousePos, -explosionDir, 1f, 1 << Layer.Blocks | 1 << Layer.Ladders | 1 << Layer.Stones).collider?.gameObject;
        
        if (block != null) {
           block.GetComponent<ICheckFallingObj>().CheckMoveDownObject();
           Destroy(block);
        }
        Destroy(bomb);
    }

}

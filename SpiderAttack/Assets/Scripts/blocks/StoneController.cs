using Assets.Scripts.enums;
using Assets.Scripts.Utils.enums;
using System.Collections;
using UnityEngine;

public class StoneController : MonoBehaviour, IMoveDown, ICheckFallingObj
{
    public float speed;
    public float delay;

    public void MoveObjectDown()
    {
        StartCoroutine(Down()); 
    }


    private IEnumerator Down()
    {
        yield return new WaitForSeconds(delay);

        Vector2 pos = transform.position;
        Vector2 direction = Vector2.up;
        float distance = 0.6f;
        //стреляем вверх и запоиманем в hit_up что сверху
        RaycastHit2D hit_up = Physics2D.Raycast(pos, direction, distance, 1 << Layer.Ladders | 1 <<Layer.Stones);

        bool trigger;
        do
        {
            trigger = false; //изначально проверка не нужна
            pos = transform.position;
            direction = Vector2.down;
            distance = 0.6f;
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << Layer.Ladders | 1 << Layer.Blocks | 1<< Layer.Stones); //стреляю вниз
            SaveHelper.Instance.DeleteObjecFromPosition(pos);

            if (hit.collider == null) //если внизу свободно, то двигаюсь вниз
            {
                Debug.Log("Я лестница! Двигаюсь вниз! Свободно!");
                transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
                trigger = true;  //нужная еще одна проверка
            }
            else
            {
                Debug.Log("Я лестница! Не могу падать, подомной стоит ");
                SaveHelper.Instance.PutObjecPosition(pos, ItemGroup.Grounds, (int)GroundCategory.BlockDynamic);
            }
        } while (trigger);


        //если сверху была не земля, то двигаем то что сверху вниз
        if (hit_up.collider != null)
            if (!hit_up.collider.name.Contains("blockGround"))
            {
                IMoveDown gm = hit_up.collider.GetComponent<IMoveDown>();
                gm.MoveObjectDown();
            }

    }

    public void CheckMoveDownObject()
    {
        var hitTop = Physics2D.Raycast(transform.position, Vector2.up, 1f, 1 << Layer.Ladders | 1 << Layer.Stones);
        if (hitTop.collider != null)
        {
            var moveDownObj = hitTop.collider.GetComponent<IMoveDown>();
            moveDownObj?.MoveObjectDown();
        }
    }
}

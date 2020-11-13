using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharStateEnemy
{
    Walk,
    Attack,
    Hurt,
    Die,
    AttackRange


}
public class KrabController : MonoBehaviour
{


    public Animator animator;
    public GameObject canvasChild;
    public int health;
    public int crit = 1;
    public int _counterAttack;
    public int counterAttackSet;
    public int _counterAttackRange;
    public int counterAttackRangeSet;
    public float a;
    bool i = false;
    public bool inWall = false;
    public bool goBack;
    public bool attackEnd;
    public bool _inHurt;
    int Direction = 1;
    float move = 1;
    public float move1 = 1f;

    //// //public Messager ms;
    // Animation anim;
    public int damageTreb;
    public int damageBalista;
    public float damageEnemy;
    private Transform wall;
    //public Text textDamage;
    public Slider slider;
    public float distance = 2f;
    public float distanceRange = 100f;
    //public GameObject CanvasChild;

    public CharStateEnemy state
    {
        get { return (CharStateEnemy)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    void Start()
    {

        animator.SetFloat("speed", move1);
        wall = GameObject.FindGameObjectWithTag("GateRight").transform;
        damageTreb = 1000;
        damageBalista = 100;

        slider.maxValue = health;
        slider.value = health;
        crit = 1;
        // health = 3000;
        animator = GetComponent<Animator>();
        //animatorText = GetComponentInChildren<Animator>();
        i = false;
        StartCoroutine(WallAttack());
    }


    void Update()

    {

        if (_inHurt == false)
        {
            transform.Translate(-Vector3.right * move * Direction * 1 * Time.deltaTime);
        }

    }
    public void CounterAttack()
    {
        _counterAttack++;
        if (_counterAttack == counterAttackSet)
        {
            StartCoroutine(MoveBack());
        }
    }
    public void CounterAttackRange()
    {
        _counterAttackRange++;
        if (_counterAttackRange == counterAttackRangeSet)
        {
            StartCoroutine(MoveBack());
        }


    }
    public void InHurt()
    {
        _inHurt = true;
    }
    public void InHurtOff()
    {
        _inHurt = false;
    }
    public void EndAttack()
    {
        attackEnd = true;
    }
    public void Attack()
    {
        attackEnd = false;
    }
    public void WallForDamage()
    {
        //wall.SendMessage("Damage", damageEnemy);
    }

    IEnumerator WallAttack()
    {

        while (true)
        {
            //Debug.Log(Vector3.Distance(transform.position, wall.position));

            if (Vector3.Distance(transform.position, wall.position) < distance)
            {



                move = 0;
                state = CharStateEnemy.Attack;
               
                
                counterAttackSet = (int)Random.Range(2f, 5f);
                //StartCoroutine(MoveBack());
                break;

            }
            
            yield return new WaitForSeconds(0.1f);
            
        }

    }
    IEnumerator WallAttackRange()
    {

        while (true)
        {

            if (Vector3.Distance(transform.position, wall.position) < distanceRange)
            {
                Debug.Log("Strelyay");
                move = 0;
                state = CharStateEnemy.AttackRange;
                //animator.SetBool("Bool", true);
                //Debug.Log("атакую");
                counterAttackRangeSet = (int)Random.Range(2f, 5f);
                //StartCoroutine(MoveBack());
                break;

            }
            yield return new WaitForSeconds(0.1f);

        }

    }
    IEnumerator MoveBack()
    {
        
        yield return new WaitForSeconds(0);



        _counterAttack = 0;
        _counterAttackRange = 0;
            Direction = -1;
            move = move1;
            animator.SetFloat("speed", move1);
            slider.transform.localScale = new Vector3(-0.01f, 0.01f, 1);
            //textDamage.transform.localScale = new Vector3(-0.01f, 0.01f, 1);
            
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            state = CharStateEnemy.Walk;
            

            StartCoroutine(MoveForward());
        
    }
    IEnumerator MoveForward()
    {

        yield return new WaitForSeconds(3);

        
        
            //transform.localScale = new Vector3(-0.35f, 0.35f, 0.35f);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            slider.transform.localScale = new Vector3(0.01f, 0.01f, 1);
            //textDamage.transform.localScale = new Vector3(0.01f, 0.01f, 1);
            Direction = 1;
            animator.SetFloat("speed", move1);
            move = move1;

            a = Random.Range(0f, 1f);
        //StartCoroutine(WallAttack());
        if (a >= 0.5f)
        {
            StartCoroutine(WallAttack());
        }
        else StartCoroutine(WallAttackRange());

    }
    public void OnCollisionEnter2D(Collision2D coll)
    {
        //if (coll.collider.tag == "TurretRight")
        //{
        //    //Debug.Log("СТЕНА");
        //    move = 0;
        //    if (move == 0)
        //    {
        //        animator.Play("hit");
        //    }
        //    StartCoroutine(AI());

        //}


        if (coll.collider.tag == "Bullet")
        {

            if (health > 0)
            {
                animator.SetTrigger("hurt");
                //state = CharStateEnemy.Hurt;
                

                //move = 0;

                //StartCoroutine(AnimWalk());
                health -= damageTreb;
                //textDamage.text = damageTreb.ToString();

                canvasChild.GetComponent<Animator>().Play("textDamage");

                slider.value -= damageTreb;
                if (health <= 0)
                {
                    StopAllCoroutines();
                    health = 0;
                    move = 0;
                    state = CharStateEnemy.Die;
                    
                    Destroy(gameObject, 4f);

                }

            }
        }
        if (coll.collider.tag == "Arrow")
        {

            if (health > 0)
            {
                state = CharStateEnemy.Hurt;
                //animator.Play("TakeDamage");
                move = 0;
                StartCoroutine(AnimWalk());
                health -= damageBalista;
                slider.value -= damageBalista;
                if (health <= 0)
                {
                    StopAllCoroutines();
                    health = 0;
                    move = 0;
                    state = CharStateEnemy.Die;
                    // animator.Play("die");
                    Destroy(gameObject, 4f);

                }

            }
        }
    }


    IEnumerator AnimWalk()
    {
        yield return new WaitForSeconds(0.5f);
        move = move1;
        state = CharStateEnemy.Walk;
        //animator.Play("walk");

    }
}

using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public enum SpiderState
    {
        Idle = 0,
        Walk = 1,
        Hurt = 2,
        Melee = 3,
        Range = 4
    }
    
    public Transform targetPos;

    public Transform webTarget; 
    public Transform gate;
    private Animator animator;
    [SerializeField] private SkeletonMecanim _skelAnim;
    private float _distanceRange = 100f;
    private bool _rightDirect;
    private bool _finishAttackAnimation = true;
    private bool _finishHurtAnimation = true;

    [SerializeField] private int hp = 1000;


    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;


    public SpiderState State
    {
        get { return (SpiderState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        _skelAnim = GetComponent<SkeletonMecanim>();
        _skelAnim.skeleton.FindSlot("body_pattern1").SetColor(Color.red);
       


        StartCoroutine(SmoothLerpLeft(300f, -Vector3.right, Random.Range(2,4)));

    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private IEnumerator SmoothLerpLeft(float time, Vector3 vector3, float targetDistance = 2)
    {
        //State = SpiderState.Walk;
        CheckFlip(vector3);
        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position + (vector3* time*2);
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            while (!_finishAttackAnimation | !_finishHurtAnimation)
            {
                yield return null;
            }

            if (Vector3.Distance(transform.position, targetPos.position) < targetDistance)
            {
                if (targetDistance <= 2)
                {
                    State = SpiderState.Melee;
                }
                else
                {
                    webTarget.position = gate.position;
                    State = SpiderState.Range;
                }

                var range = Random.Range(4, 6);
                yield return new WaitForSeconds(range);

 
                StartCoroutine(SmoothLerpRight(Random.Range(1,4), Vector3.right));
                break;
            }
            State = SpiderState.Walk;
            transform.position = Vector3.Lerp(startingPos, finalPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
    private IEnumerator SmoothLerpRight(float time, Vector3 vector3)
    {
        //while (!_finishAttackAnimation | !_finishHurtAnimation)
        //{
        //    yield return null;
        //}

   


         CheckFlip(vector3);
        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position + (vector3 * time*2);
        float elapsedTime = 0;

        if (!_finishAttackAnimation)
        {

        }

        while (elapsedTime < time)
        {
            while (!_finishHurtAnimation)
            {
                yield return null;
            }
            State = SpiderState.Walk;
            transform.position = Vector3.Lerp(startingPos, finalPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SmoothLerpLeft(100, Vector3.left, Random.Range(2,4)));
    }

    IEnumerator Wait()
    {
        var track = skeletonAnimation.state.SetAnimation(0, "attack_range", false);
        yield return new WaitForSpineAnimationComplete(track);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BulletStone")
        {
            _finishHurtAnimation = false;
            _finishAttackAnimation = true;
            //State = SpiderState.Hurt;
            animator.SetTrigger("Hurt");
            Destroy(collision.gameObject);

        }
    }

    private void CheckFlip(Vector3 vector3)
    {
        if (vector3 == Vector3.left && _rightDirect) Flip();
        if (vector3 == Vector3.right && !_rightDirect) Flip();
    }
    private void Flip()
    {
        _rightDirect = !_rightDirect;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void FinishAttackAnimation()
    {
        Debug.Log("Finish");
        _finishAttackAnimation = true;
    }
    public void StartAttackAnimation()
    {
        Debug.Log("Start");
        _finishAttackAnimation = false;
    }

    //public void StartHurt()
    //{
    //    _finishHurtAnimation = false;
    //}

    public void FinishHurt()
    {
        _finishHurtAnimation = true;
    }
}

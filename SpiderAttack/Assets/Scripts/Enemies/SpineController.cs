using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;

public class SpineController : MonoBehaviour
{

#if UNITY_4_5
    [Header("Graphics")]
#endif
    public SkeletonAnimation skeletonAnimation;
#if UNITY_4_5
    [Header("Animation")]
#endif
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string idle = "Idle";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string walk = "Walk";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string attack_melee = "attack_melee";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string attack_range = "attack_range";
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string hurt = "Hurt"; 

    public Transform targetPos;
    public Transform webTarget;
    public Transform gate;
    private bool _rightDirect;

    private bool attackComplete = false;
    private string _currentAnim;
    public float speed = 2;
    private Slot _webSlot;
    private Slot _smear;
    public List<Coroutine> _coroutines ;
    private bool _isHurt;



    void Awake()
    {
        _coroutines= new List<Coroutine>();
        //_controller = GetComponent<MyController>();
    }

    // Use this for initialization
    void Start()
    {
        _webSlot = skeletonAnimation.skeleton.FindSlot("web");
        _smear = skeletonAnimation.skeleton.FindSlot("smear");
        skeletonAnimation.skeleton.FindSlot("body_pattern1").SetColor(Color.red);

        StopAnimCoroutines();
        var co = StartCoroutine(SmoothLerpLeft( -Vector3.right, Random.Range(2, 2)));
        _coroutines.Add(co);
    }

    private IEnumerator SmoothLerpLeft( Vector3 vector3, float targetDistance = 2)
    {
        CheckFlip(vector3);

        var animWalk =  skeletonAnimation.state.SetAnimation(0, walk, true);
        _currentAnim = walk;
        while (true)
        {
            if (Vector3.Distance(transform.position, targetPos.position) < targetDistance)
            {
                if (targetDistance <= 2)
                {
                    var animMelee = skeletonAnimation.state.SetAnimation(0, attack_melee, true);
                    _currentAnim = attack_melee;
                    animMelee.Complete += x=> attackComplete = true;
                }
                else
                {
                    webTarget.position = gate.position;
                    var animRange = skeletonAnimation.state.SetAnimation(0, attack_range, true);
                    _currentAnim = attack_range;
                    animRange.Complete += x => attackComplete = true;
                }

                var range = Random.Range(4, 6);
                yield return new WaitForSeconds(range);

                if (!_isHurt)
                {
                    StopAnimCoroutines();
                    var co = StartCoroutine(SmoothLerpRight(Random.Range(1, 4), Vector3.right));
                    _coroutines.Add(co);
                }


                break;
            }

            transform.Translate(vector3 * speed * Time.deltaTime);
            yield return null;
        }

    }

    private IEnumerator SmoothLerpRight(float time, Vector3 vector3)
    {
        while (!attackComplete)
        {
            yield return null;
        }
        attackComplete = false;
        CheckFlip(vector3);

        float elapsedTime = 0;

        skeletonAnimation.state.SetAnimation(0, walk, true);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.Translate(vector3 * speed* Time.deltaTime);
            yield return null;
        }

        if (!_isHurt)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(SmoothLerpLeft(Vector3.left, Random.Range(2, 2)));
            _coroutines.Add(co);

        }

    }

    void Update()

    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BulletStone")
        {
            _webSlot.A = 0;
            //_smear.A = 0;
            speed = 0;
            var animHurt = skeletonAnimation.state.SetAnimation(0, hurt, false);
            _isHurt = true;
            animHurt.Complete += SpineController_Complete;
            animHurt.TimeScale = 1.5f;

            EventManager.Instance.PostNotification(EVENT_TYPE.SpiderHurt, this, collision.gameObject.GetComponent<TrebuchetBullet>().Damage);

            //skeletonAnimation.state.Event += delegate (Spine.TrackEntry entry, Spine.Event e)
            //{
            //    Debug.Log("Custom event fired for track: " + entry.TrackIndex + " with event name: " + e.Data.Name);
            //};

            Destroy(collision.gameObject);

        }
    }

    private void SpineController_Complete(TrackEntry trackEntry)
    {


        attackComplete = true;
        speed = 2;
        _webSlot.A = 1;
        //_smear.A = 1;

        if (_currentAnim == attack_melee || _currentAnim == attack_range)
        {
            StopAnimCoroutines();
            var co = StartCoroutine(SmoothLerpRight(Random.Range(1, 4), Vector3.right));
            _coroutines.Add(co);
        }

        if (_currentAnim == walk)
        {
            skeletonAnimation.state.SetAnimation(0, walk, true);
        }

        _isHurt = false;
    }

    //private void StartAnimCoroutine(float range, Vector3 direction)
    //{
    //    StopAnimCoroutines();
    //    var co = StartCoroutine(SmoothLerpRight(Random.Range(1, 4), Vector3.right));
    //    _coroutines.Add(co);
    //}

    private void StopAnimCoroutines()
    {
        if (!_coroutines.Any())
        {
            return;
        }
        foreach (var coroutine in _coroutines)
        {
            StopCoroutine(coroutine);
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

}
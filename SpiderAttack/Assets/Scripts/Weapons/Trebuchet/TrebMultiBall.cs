using MarchingBytes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebMultiBall : MonoBehaviour
{
    public TrebChildBall[] replicates;
    private Rigidbody2D[] replicatesRbs;
    public Vector3[] directions;
    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);
    public int speed;
    public float explosionDelay = 2f;
    private SpriteRenderer _bulletSpriteRenderer;
    private Rigidbody2D _rb;

    public GameObject explosion;

    //public float timeRemaining = 10;
    public bool timerIsRunning = false;

    public int _childCounter;

    public int ChildCounter {
        get => _childCounter;
        set {
            _childCounter = value;
            if(_childCounter > replicates.Length - 1)
            {
               // EasyObjectPool.instance.ReturnObjectToPool(gameObject);
            }
        }
    }

    private void Awake()
    {
        _bulletSpriteRenderer = GetComponent<SpriteRenderer>();
        replicatesRbs = new Rigidbody2D[replicates.Length];
        for (int i = 0; i < replicates.Length; i++)
        {
            replicatesRbs[i] = replicates[i].GetComponent<Rigidbody2D>();
        }
    }

    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (explosionDelay > 0)
            {
                explosionDelay -= Time.deltaTime;
            }
            else
            {
                Dublication();
                _bulletSpriteRenderer.enabled = false;
                explosion.SetActive(true);
                _rb.bodyType = RigidbodyType2D.Static;

                explosionDelay = 0;
                timerIsRunning = false;
            }
        }
    }

    private void Dublication()
    {
        for (int i = 0; i < replicates.Length; i++)
        {
            replicates[i].gameObject.transform.localPosition = vectorZero;
            replicates[i].gameObject.transform.localRotation = quatZero;

            replicates[i].StartArrow();
            replicatesRbs[i].AddRelativeForce(directions[i] * speed);
        }
    }

    public void OnEnable()
    {
        ChildCounter = 0;
        //explosionDelay = 2f;
        timerIsRunning = true;
        //Invoke("Dublication", explosionDelay);
        //_bulletSpriteRenderer.enabled = false;
        //Dublication();
    }

    public void ShotDelay()
    {

    }
}

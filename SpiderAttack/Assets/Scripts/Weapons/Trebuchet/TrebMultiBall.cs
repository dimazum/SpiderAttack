using MarchingBytes;
using UnityEngine;

public class TrebMultiBall : Bullet
{
    public TrebChildBall[] replicates;
    private Rigidbody2D[] replicatesRbs;
    public Vector3[] directions;
    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);
    public int speed;
    public float explosionDelay = 1.7f;
    private SpriteRenderer _bulletSpriteRenderer;
    private Rigidbody2D _rb;
    public Animation animation;
    public bool timerIsRunning ;
    public int _childCounter;
    private float explosionDelayCounter;
    private EasyObjectPool _easyObjectPool;

    public int ChildCounter
    {
        get => _childCounter;
        set
        {
            _childCounter = value;
            if (_childCounter > replicates.Length - 1)
            {
                _easyObjectPool.ReturnObjectToPool(gameObject);
                BackToStartState();
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        _easyObjectPool.ReturnObjectToPool(gameObject); ;
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
        _easyObjectPool = transform.parent.GetComponentInParent<EasyObjectPool>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (explosionDelayCounter > 0)
            {
                explosionDelayCounter -= Time.deltaTime;
            }
            else
            {
                Duplication();
                _bulletSpriteRenderer.enabled = false;
                animation.Play();
                _rb.bodyType = RigidbodyType2D.Static;

                explosionDelayCounter = explosionDelay;
                timerIsRunning = false;
            }
        }
    }

    private void Duplication()
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
        explosionDelayCounter = explosionDelay;
        timerIsRunning = true;
    }

    private void BackToStartState()
    {
        gameObject.transform.localPosition = vectorZero;
        gameObject.transform.localRotation = quatZero;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _bulletSpriteRenderer.enabled = true;
    }
}

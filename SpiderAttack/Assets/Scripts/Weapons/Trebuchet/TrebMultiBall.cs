using MarchingBytes;
using UnityEngine;

public class TrebMultiBall : BaseBall
{
    [SerializeField]
    public TrebChildBall[] replicates;
    private Rigidbody2D[] replicatesRbs;
    public Vector3[] directions;
    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);
    public int speed;
    public float explosionDelay = 1.7f;
    private SpriteRenderer _bulletSpriteRenderer;
    [SerializeField]
    private SpriteRenderer[] mockSprites;
    private Rigidbody2D _rb;
    public Animation animation;
    public bool timerIsRunning ;
    public int _childCounter;
    private float explosionDelayCounter;
    private EasyObjectPool _easyObjectPool;
    private bool _dublicationRun;

    public int _hitSpiderCounter;

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

                //if (_hitSpiderCounter > 0)
                //{
                //    EventManager.Instance.PostNotification(EVENT_TYPE.BallHitTarget, this);
                //}
                if (_hitSpiderCounter == 0)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.ArrowMissedTarget, this);
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //_easyObjectPool.ReturnObjectToPool(gameObject); 
        Duplication();
        _bulletSpriteRenderer.enabled = false;
        UpdateMockSprites(false);

        animation.Play();
        _rb.bodyType = RigidbodyType2D.Static;
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
                transform.Rotate(0, 0, -.5f, Space.Self);
            }
            else
            {
                Duplication();
                _bulletSpriteRenderer.enabled = false;
                UpdateMockSprites(false);

                animation.Play();
                _rb.bodyType = RigidbodyType2D.Static;

                explosionDelayCounter = explosionDelay;
                timerIsRunning = false;
                _dublicationRun = false;
            }
        }
       
    }

    private void UpdateMockSprites(bool isEnabled)
    {
        for (int i = 0; i < mockSprites.Length; i++)
        {
            mockSprites[i].enabled = isEnabled;
        }
    }

    private void Duplication()
    {
        if (_dublicationRun) return;
        _dublicationRun = true;
        //_childCounter = 0;
        _hitSpiderCounter = 0;
        for (int i = 0; i < replicates.Length; i++)
        {
            //replicates[i].GetComponent<SpriteRenderer>().sprite = _bulletSpriteRenderer.sprite;
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
        UpdateMockSprites(true);
    }
}

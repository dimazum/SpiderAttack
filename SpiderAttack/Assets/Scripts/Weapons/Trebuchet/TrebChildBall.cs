using Assets.Scripts.enums;
using UnityEngine;

public class TrebChildBall : Bullet
{
    public TrebMultiBall trebMiltyBall;
    public Animation animation;
    public GameObject explosionObj;
    public float explosionDelay = 1f;
    public bool timerIsRunning = false;

    private Vector3 vectorZero = new Vector3(0, 0, 0);
    private Quaternion quatZero = new Quaternion(0, 0, 0, 0);


    public void StartArrow()
    {
        Damage = trebMiltyBall.Damage;
        gameObject.SetActive(true);
        //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        explosionObj.transform.SetParent(null);
        animation.Play();
        timerIsRunning = true;
        gameObject.layer = Layer.Dead;
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
                explosionDelay = 1f;
                timerIsRunning = false;
                BackToStartState();
            }
        }
    }

    private void BackToStartState()
    {
        gameObject.SetActive(false);
        //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.layer = Layer.BulletStone;
        gameObject.transform.localPosition = vectorZero;
        gameObject.transform.localRotation = quatZero;
        explosionObj.transform.SetParent(gameObject.transform);
        explosionObj.transform.localPosition = vectorZero;
        explosionObj.transform.localRotation = quatZero;
        trebMiltyBall.ChildCounter++;
    }
}
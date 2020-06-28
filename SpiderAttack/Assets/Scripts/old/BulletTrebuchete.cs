using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrebuchete : MonoBehaviour {

    public GameObject Exspl;
    public Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == true)
        {
            Instantiate(Exspl, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

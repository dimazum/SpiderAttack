using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebChildBall : TrebuchetBullet
{
    public TrebMultiBall ballistaTripleArrow;
    public GameObject _animator;

    private void Start()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.transform.SetParent(null);
        _animator.SetActive(true);
        gameObject.SetActive(false);
        ballistaTripleArrow.ChildCounter++;
    }

    public void StartArrow()
    {
        gameObject.SetActive(true);
    }
}
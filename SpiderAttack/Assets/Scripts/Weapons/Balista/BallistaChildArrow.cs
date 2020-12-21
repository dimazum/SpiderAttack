using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaChildArrow : BalistaArrow
{
    public BallistaMultiArrow ballistaTripleArrow;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        ballistaTripleArrow.ChildCounter++;   
    }

    public void StartArrow()
    {
        gameObject.SetActive(true);
    }
}

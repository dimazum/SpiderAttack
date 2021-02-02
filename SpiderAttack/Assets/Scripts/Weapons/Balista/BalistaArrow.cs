
using UnityEngine;

public class BalistaArrow : BaseArrow
{
    public Transform centerOfMass;
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
        rb.centerOfMass = centerOfMass.position;
    }
}

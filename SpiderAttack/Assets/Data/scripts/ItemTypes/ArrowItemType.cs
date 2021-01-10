using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowItemType : BaseItemType, ICanBeInStock
{
    public ArrowCategory arrowCategory;
    [SerializeField]private int _qtyInStock;

    public int QtyInStock 
    {
        get { return _qtyInStock; }
        set { _qtyInStock = value; } 
    }
}

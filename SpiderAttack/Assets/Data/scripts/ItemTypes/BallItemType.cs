using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallItemType : BaseItemType, ICanBeInStock
{
    //public int QtyInStock;
    public BallCategory ballCategory;

    public int QtyInStock { get ; set ; }
}

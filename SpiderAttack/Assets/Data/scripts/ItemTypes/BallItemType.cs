using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseItemType : MonoBehaviour
{
    [SerializeField] private int _qty;
    public int Qty
    {
        get => _qty;
        set
        {
            if (value >= 0)
            {
                _qty = value;
            }
        }
    }
    public int QtyInStock;
    public bool endlesQty;
    public Sprite image;
}

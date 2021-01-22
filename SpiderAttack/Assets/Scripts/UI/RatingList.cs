using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingList : MonoBehaviour
{
    //public Transform container;
    public RectTransform rectTransform;

    void Update()
    {
        var test = rectTransform.rect;
        var h = test.height;
        Debug.Log(h);

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgratesPanelController : MonoBehaviour
{
    private int _childIndex = 0;
    private int _childQty;
    private Transform _currentShop;
    void Start()
    {
        _childQty = gameObject.transform.childCount;
        EnableShop(0);
    }

    void EnableShop(int childIndex)
    {
        _currentShop = gameObject.transform.GetChild(childIndex);
        _currentShop.gameObject.SetActive(true);
    }

    public void NextShop()
    {
        if (_childIndex < _childQty-1)
        {
            _currentShop.gameObject.SetActive(false);
            _childIndex++;
            //_currentShop = gameObject.transform.GetChild(_childIndex);
            //_currentShop.gameObject.SetActive(true);
            EnableShop(_childIndex);
        }
        
    }

    public void PreviousShop()
    {

        if (_childIndex > 0)
        {
            _currentShop.gameObject.SetActive(false);
            _childIndex--;
            //_currentShop = gameObject.transform.GetChild(_childIndex);
            //_currentShop.gameObject.SetActive(true);
            EnableShop(_childIndex);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharackterHelper : Singleton<CharackterHelper>
{
    [SerializeField]
    private GameObject _mainChar;

    void Start()
    {
        
    }

    public bool CheckIfCharIsInCave()
    {
        var pos = _mainChar.transform.position;
        if (-20f < pos.x && pos.x < 30)
        {
            if (-0.5f < pos.y && pos.y < 10)
            {
                return false;
            }
        }
        return true;
    }
}

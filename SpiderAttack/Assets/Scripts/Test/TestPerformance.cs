using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestPerformance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int numTests = 1000000;
        TestComponent test;

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < numTests; ++i)
        {
            //var anim = GetComponent("Animator");
            //var anim = GetComponent<Animator>();
            var anim = GetComponent(typeof(Animator));
        }
        TimeSpan ts = stopWatch.Elapsed;
        UnityEngine.Debug.Log(ts);
    }

    // Update is called once per frame
    void Update()
    {

    }
}


public class TestComponent : Component
{

}

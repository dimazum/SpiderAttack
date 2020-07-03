using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<Slider>().value = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

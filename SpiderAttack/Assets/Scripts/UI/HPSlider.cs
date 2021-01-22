using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<Slider>().value = 1;
    }


}

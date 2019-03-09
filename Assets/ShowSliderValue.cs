using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowSliderValue : MonoBehaviour
{
    Slider sensitivitySlider = null;
    Text thisText = null;
    // Start is called before the first frame update
    void Start()
    {
        sensitivitySlider = transform.GetComponentInParent<Slider>();
        thisText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = sensitivitySlider.value.ToString();
    }
}

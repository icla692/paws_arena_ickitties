using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
    public Slider slider;
    public float GetValue()
    {
        return slider.value;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}

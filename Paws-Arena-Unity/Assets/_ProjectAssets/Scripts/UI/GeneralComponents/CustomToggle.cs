using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomToggle : MonoBehaviour
{
    public bool isOn = true;
    public RectTransform toggleBg;
    public RectTransform toggleCircle;

    [Header("UI")]
    public Color onColor;
    public Color offColor;
    public float circleOnPosX = 20;
    public float circleOffPosX = 80;
    private void Start()
    {
        SetValue(isOn);
    }


    public void SwitchValue()
    {
        SetValue(!isOn);
    }

    public void SetValue(bool isOn)
    {
        this.isOn = isOn;

        float time = .3f;

        if (isOn)
        {
            LeanTween.color(toggleBg, onColor, time);
            LeanTween.color(toggleCircle, onColor, time);
            LeanTween.moveX(toggleCircle, circleOnPosX, time);
        }
        else
        {
            LeanTween.color(toggleBg, offColor, time);
            LeanTween.color(toggleCircle, offColor, time);
            LeanTween.moveX(toggleCircle, circleOffPosX, time);
        }
    }
}

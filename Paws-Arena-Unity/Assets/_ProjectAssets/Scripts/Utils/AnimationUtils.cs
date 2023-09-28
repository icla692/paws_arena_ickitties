using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtils
{
    public static void GrowValue(TMPro.TextMeshProUGUI text, int from, int to, float time)
    {
        LeanTween.value(from, to, time).setOnUpdate((float val) =>
        {
            text.text = "" + Mathf.Floor(val);
        });
    }
}

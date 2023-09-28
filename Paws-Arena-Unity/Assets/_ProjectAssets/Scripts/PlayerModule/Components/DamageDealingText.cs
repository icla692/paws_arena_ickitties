using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealingText : MonoBehaviour
{
    public static Action Finished;
    public void Init(int damage)
    {
        var text = GetComponent<TMPro.TextMeshPro>();
        text.text = $"-{damage}p";
        text.color = Color.clear;

        var rect = GetComponent<RectTransform>();
        LeanTween.value(0, 1, 0.5f).setDelay(UnityEngine.Random.Range(0, 0.5f)).setOnComplete(() =>
        {
            text.color = Color.white;
            Color _color = text.color;
            LeanTween.value(0, 2, 1.5f).setOnUpdate(val =>
            {
                rect.anchoredPosition = new Vector2(0, val);
                _color.a = 1 - (val / 2);
                text.color = _color;

            }).setOnComplete(() =>
            {
                Destroy(gameObject.transform.parent.gameObject);
                Finished?.Invoke();
            });
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointScrollbar : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float maxY;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _rectTransform.anchoredPosition = new Vector2(0, Mathf.Clamp(maxY * (1 - scrollbar.value), maxY, 0));
    }
}

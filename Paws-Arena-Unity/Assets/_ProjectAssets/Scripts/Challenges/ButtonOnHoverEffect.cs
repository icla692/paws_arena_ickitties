using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonOnHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color onHoverColor;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        image.color = normalColor;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = onHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }
}

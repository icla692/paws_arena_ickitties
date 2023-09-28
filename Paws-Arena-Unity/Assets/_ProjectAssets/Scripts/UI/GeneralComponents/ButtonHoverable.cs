using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultSprite;

    public Color colorOverlay;
    public TMPro.TextMeshProUGUI text;

    public Sprite onClickImage;

    private Button button;
    private Color initColor;

    private bool isLocked = false;

    private void Start()
    {
        button = GetComponent<Button>();
        initColor = button.image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;

        button.image.color = colorOverlay;
        if(text!= null)
        {
            text.color = colorOverlay;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;

        button.image.color = initColor;
        if (text != null)
        {
            text.color = initColor;
        }
    }

    public void Select()
    {
        isLocked = true;

        button.image.sprite = onClickImage;
        button.image.color = initColor; 
    }

    public void Deselect()
    {
        isLocked = false;
        button.image.sprite = defaultSprite;
        button.image.color = initColor;
    }
}

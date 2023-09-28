using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFTImageSprite : MonoBehaviour
{
    public event Action onClick;

    public Image mainImage;

    public Image selectionImage;
    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickBehaviour);    
    }

    private void OnClickBehaviour()
    {
        onClick?.Invoke();
    }

    public void Select()
    {
        selectionImage.sprite = selectedSprite;
    }

    public void Deselect()
    {
        selectionImage.sprite = deselectedSprite;
    }
}

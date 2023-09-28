using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisenchantedItemDisplay : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Image iconDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;

    public void Setup(Sprite _sprite)
    {
        closeButton.onClick.AddListener(Close);

        nameDisplay.text = _sprite.name;
        iconDisplay.sprite = _sprite;
        iconDisplay.SetNativeSize();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}

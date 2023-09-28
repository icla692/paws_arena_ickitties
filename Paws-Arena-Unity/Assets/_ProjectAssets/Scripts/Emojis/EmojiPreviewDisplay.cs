using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EmojiPreviewDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Action<int> OnEmojiClicked;
    
    [SerializeField] private Image emojiDisplay;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightedColor;
    
    private EmojiSO emoji;
    private bool isShowing;
    
    public void Setup(EmojiSO _emoji)
    {
        emoji = _emoji;
        emojiDisplay.sprite = emoji.Sprite;
        emojiDisplay.transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.AddListener(HandleClick);
    }

    public void Show(float _duration,float _delay)
    {
        backgroundImage.color = normalColor;
        emojiDisplay.transform.localScale = Vector3.zero;
        Vector3 _targetScale = Vector3.one;
        
        LeanTween.scale(emojiDisplay.gameObject, _targetScale, _duration)
            .setEase(LeanTweenType.easeOutCubic) // Use a desired easing type
            .setDelay(_delay);
    }

    public void Hide(float _duration,float _delay)
    {
        LeanTween.cancel(gameObject);
        emojiDisplay.transform.localScale = Vector3.one;
        Vector3 _targetScale = Vector3.zero;

        LeanTween.scale(emojiDisplay.gameObject, _targetScale, _duration)
            .setEase(LeanTweenType.easeOutCubic) // Use a desired easing type
            .setDelay(_delay);
    }

    private void HandleClick()
    {
        OnEmojiClicked?.Invoke(emoji.Id);
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        backgroundImage.color = highlightedColor;
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        backgroundImage.color = normalColor;
    }
}

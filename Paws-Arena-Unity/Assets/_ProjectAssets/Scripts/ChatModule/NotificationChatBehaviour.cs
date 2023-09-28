using UnityEngine;

public class NotificationChatBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Play a sound FX for chat notification");
        LeanTween.scale(gameObject, Vector2.one * 2, 1f).setEase(LeanTweenType.punch);
    }
}

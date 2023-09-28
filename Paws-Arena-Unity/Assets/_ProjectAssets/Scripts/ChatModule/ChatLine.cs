using TMPro;
using UnityEngine;

public class ChatLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(string message, bool isLocal)
    {
        text.alignment = isLocal ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
        text.text = message;
    }
}

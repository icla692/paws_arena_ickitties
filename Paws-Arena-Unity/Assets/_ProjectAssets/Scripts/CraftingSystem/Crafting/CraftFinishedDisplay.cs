using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftFinishedDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Button claimButton;

    public void Setup(string _text)
    {
        textDisplay.text = _text;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        claimButton.onClick.AddListener(Claim);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(Claim);
    }

    private void Claim()
    {
        gameObject.SetActive(false);
    }
}

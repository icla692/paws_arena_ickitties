using UnityEngine;
using TMPro;

public class RecoveryMessageDisplay : MonoBehaviour
{
    public static RecoveryMessageDisplay Instance;

    [SerializeField] private GameObject recoveryMessageHolder;
    [SerializeField] private GameObject recoveryMessageBackground;
    [SerializeField] private TextMeshProUGUI recoveryText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMessage()
    {
        recoveryText.text = $"Your Kitty is not fully healed yet. Wait {GameState.selectedNFT.MinutesUntilHealed} minutes or buy a Milk Bottle to heal it.";
        recoveryMessageBackground.transform.localScale = Vector3.zero;
        recoveryMessageHolder.SetActive(true);
        recoveryMessageBackground.LeanScale(Vector3.one, 0.5f);
    }
}

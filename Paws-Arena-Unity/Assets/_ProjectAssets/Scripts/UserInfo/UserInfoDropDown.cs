using UnityEngine;
using UnityEngine.UI;

public class UserInfoDropDown : MonoBehaviour
{
    [SerializeField] private Button seasonButton;
    [SerializeField] private Button switchKittyButton;
    [SerializeField] private LobbyUIManager lobbyUI;
    [SerializeField] private LevelsPanel levelsPanel;
    [SerializeField] private RecoveryDropDown recoveryDropDown;

    private float animationLength = 0.1f;
    private bool isOpen;

    private void OnEnable()
    {
        seasonButton.onClick.AddListener(ShowSeason);
        switchKittyButton.onClick.AddListener(SwitchKitty);
        Close();
    }

    private void OnDisable()
    {
        seasonButton.onClick.RemoveListener(ShowSeason);
        switchKittyButton.onClick.RemoveListener(SwitchKitty);
    }

    public void HandleClick()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            recoveryDropDown.Close();
            Show();
        }
    }

    public void Close()
    {
        gameObject.LeanScale(Vector3.zero, animationLength);
        isOpen = false;
    }

    public void Show()
    {
        gameObject.LeanScale(Vector3.one, animationLength);
        isOpen = true;
    }

    public void SwitchKitty()
    {
        lobbyUI.OpenNFTSelectionScreen();
        Close();
    }

    public void ShowSeason()
    {
        levelsPanel.Setup();
        Close();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ShowGuild : MonoBehaviour
{
    [SerializeField] private GuildPanel guildPanel;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(FetchGuild);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(FetchGuild);
    }

    private void FetchGuild()
    {
        button.interactable = false;
        FirebaseManager.Instance.CheckIfPlayerIsStillInGuild(Show);
    }

    private void Show()
    {
        button.interactable = true;
        guildPanel.Setup();
    }
}

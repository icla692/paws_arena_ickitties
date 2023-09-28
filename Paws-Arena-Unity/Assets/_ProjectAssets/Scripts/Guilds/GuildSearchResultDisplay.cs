using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuildSearchResultDisplay : MonoBehaviour
{
    public static Action<GuildData> OnJoinGuild;
    
    [SerializeField] private Image badgeDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private Image kittyDisplay;
    [SerializeField] private TextMeshProUGUI amountOfMembersDisplay;
    [SerializeField] private TextMeshProUGUI minPoints;
    [SerializeField] private Button joinButton;

    private GuildData guildData;
    
    public void Setup(GuildData _guildData)
    {
        GuildSO _guildSO = GuildSO.Get(_guildData.FlagId);
        guildData = _guildData;
        badgeDisplay.sprite = _guildSO.Badge;
        nameDisplay.text = guildData.Name;
        kittyDisplay.sprite = _guildSO.Kitty;
        amountOfMembersDisplay.text = guildData.Players.Count.ToString();
        minPoints.text = _guildData.MinimumPoints.ToString();
    }

    private void OnEnable()
    {
        joinButton.onClick.AddListener(JoinGuild);
    }

    private void OnDisable()
    {
        joinButton.onClick.RemoveListener(JoinGuild);
    }

    private void JoinGuild()
    {
        OnJoinGuild?.Invoke(guildData);
    }
}

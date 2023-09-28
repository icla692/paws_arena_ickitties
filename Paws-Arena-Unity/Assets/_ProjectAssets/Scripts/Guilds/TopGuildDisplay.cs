using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TopGuildDisplay : MonoBehaviour
{
    public static Action<GuildData> OnJoinGuild;
    
    [SerializeField] private Image badgeDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private Image kittyDisplay;
    [SerializeField] private TextMeshProUGUI amountOfMembersDisplay;
    [SerializeField] private TextMeshProUGUI sumOfPoints;
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
        sumOfPoints.text = guildData.SumOfPoints.ToString();
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

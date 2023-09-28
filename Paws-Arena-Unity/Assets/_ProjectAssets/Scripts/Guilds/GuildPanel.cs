using UnityEngine;

public class GuildPanel : GuildPanelBase
{
    [SerializeField] private NoGuildPanel noGuildPanel;
    [SerializeField] private GuildBattlePanel guildBattlePanel;
    [SerializeField] private CreateGuildPanel createGuildPanel;
    [SerializeField] private JoinGuildPanel joinGuildPanel;
    [SerializeField] private HasGuildPanel hasGuildPanel;
    [SerializeField] private GuildTopGuildsPanel topGuildPanel;
    [SerializeField] private SearchGuilds searchGuilds;

    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GuildLeftPanel.OnShowMyGuild += ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle += ShowGuildBattle;
        GuildLeftPanel.OnClose += Close;
        GuildLeftPanel.OnShowTopGuilds += ShowTopGuilds;
        GuildLeftPanel.OnShowSearchGuilds += OnShowSearchGuilds;
        NoGuildPanel.OnShowCreateGuild += ShowCreateGuild;
        NoGuildPanel.OnShowJoinGuild += ShowJoinGuild;
        DataManager.Instance.PlayerData.UpdatedGuild += ShowMyGuild;
        JoinGuildPanel.OnJoinedGuild += ShowMyGuild;
        SearchGuilds.OnJoinedGuild += ShowMyGuild;
    }

    private void OnDisable()
    {
        GuildLeftPanel.OnShowMyGuild -= ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle -= ShowGuildBattle;
        GuildLeftPanel.OnClose -= Close;
        GuildLeftPanel.OnShowTopGuilds -= ShowTopGuilds;
        GuildLeftPanel.OnShowSearchGuilds -= OnShowSearchGuilds;
        NoGuildPanel.OnShowCreateGuild -= ShowCreateGuild;
        NoGuildPanel.OnShowJoinGuild -= ShowJoinGuild;
        DataManager.Instance.PlayerData.UpdatedGuild -= ShowMyGuild;
        JoinGuildPanel.OnJoinedGuild -= ShowMyGuild;
        SearchGuilds.OnJoinedGuild += ShowMyGuild;
    }

    private void ShowMyGuild()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            SwitchToPanel(noGuildPanel);
        }
        else
        {
            SwitchToPanel(hasGuildPanel);
        }
    }

    private void ShowTopGuilds()
    {
        SwitchToPanel(topGuildPanel);
    }

    private void OnShowSearchGuilds()
    {
        SwitchToPanel(searchGuilds);
    }

    private void ShowGuildBattle()
    {
        SwitchToPanel(guildBattlePanel);
    }

    private void ShowCreateGuild()
    {
        if (DataManager.Instance.PlayerData.IsInGuild)
        {
            return;
        }

        SwitchToPanel(createGuildPanel);
    }

    private void ShowJoinGuild()
    {
        if (DataManager.Instance.PlayerData.IsInGuild)
        {
            return;
        }

        SwitchToPanel(joinGuildPanel);
    }

    private void SwitchToPanel(GuildPanelBase _panel)
    {
        noGuildPanel.Close();
        guildBattlePanel.Close();
        createGuildPanel.Close();
        joinGuildPanel.Close();
        hasGuildPanel.Close();
        topGuildPanel.Close();
        searchGuilds.Close();

        _panel.Setup();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildTopGuildsPanel : GuildPanelBase
{
    [SerializeField] private TopGuildDisplay topGuildDisplay;
    [SerializeField] private Transform goldRankHolder;
    [SerializeField] private Transform silverRankHolder;
    [SerializeField] private Transform bronzeRankHolder;
    [SerializeField] private Transform restOfGuildsHolder;
    [SerializeField] private RectTransform[] layoutGroups;

    private List<GameObject> shownGuilds = new ();

    public override void Setup()
    {
        ClearShownGuilds();
        ShowGuilds();
        gameObject.SetActive(true);
        
        StartCoroutine(RebuildLayoutGroups());

        IEnumerator RebuildLayoutGroups()
        {
            foreach (var _layoutGroup in layoutGroups)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);
                yield return null;
            }
        }
    }

    private void ClearShownGuilds()
    {
        foreach (var _shownGuild in shownGuilds)
        {
            Destroy(_shownGuild);
        }
        
        shownGuilds.Clear();
    }

    private void ShowGuilds()
    {
        List<GuildData> _goldGuilds = new();
        List<GuildData> _silverGuilds = new();
        List<GuildData> _bronzeGuilds = new();
        List<GuildData> _restOfGuilds = new();

        foreach (var _guild in DataManager.Instance.GameData.Guilds.Values)
        {
            if (_guild.SumOfPoints >= DataManager.Instance.GameData.RankingBorders.Gold)
            {
                _goldGuilds.Add(_guild);
            }
            else if (_guild.SumOfPoints >= DataManager.Instance.GameData.RankingBorders.Silver)
            {
                _silverGuilds.Add(_guild);
            }
            else if (_guild.SumOfPoints >= DataManager.Instance.GameData.RankingBorders.Bronze)
            {
                _bronzeGuilds.Add(_guild);
            }
            else
            {
                _restOfGuilds.Add(_guild);
            }
        }

        DisplayGuilds(_goldGuilds,goldRankHolder);
        DisplayGuilds(_silverGuilds,silverRankHolder);
        DisplayGuilds(_bronzeGuilds,bronzeRankHolder);
        DisplayGuilds(_restOfGuilds,restOfGuildsHolder);

        void DisplayGuilds(List<GuildData> _guilds, Transform _holder)
        {
            foreach (var _guild in _guilds)
            {
                TopGuildDisplay _guildDisplay = Instantiate(topGuildDisplay, _holder);
                _guildDisplay.Setup(_guild);
                shownGuilds.Add(_guildDisplay.gameObject);
            }
        }
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}

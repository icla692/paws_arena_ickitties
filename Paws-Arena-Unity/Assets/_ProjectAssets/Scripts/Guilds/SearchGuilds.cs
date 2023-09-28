using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchGuilds : GuildPanelBase
{
    public static Action OnJoinedGuild;
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private GuildSearchResultDisplay searchResultDisplay;
    [SerializeField] private Transform resultsHolder;
    [SerializeField] private GameObject noGuildsMessage;
    [SerializeField] private GameObject searchWithNoResultsMessage;
    [SerializeField] private GameObject noEnaughtPointsMessage;
    [SerializeField] private Button showUp;
    [SerializeField] private Button showDown;
    [SerializeField] private GameObject alreadyInGuild;

    [SerializeField] private GameObject joiningGuildPanel;
    private List<GameObject> shownObjects = new();
    private float moveAmount = 1;
    
    private void OnEnable()
    {
        showUp.onClick.AddListener(ShowUp);
        showDown.onClick.AddListener(ShowDown);
        searchInput.onEndEdit.AddListener(Search);

        GuildSearchResultDisplay.OnJoinGuild += JoinGuild;
    }

    private void OnDisable()
    {
        showUp.onClick.RemoveListener(ShowUp);
        showDown.onClick.RemoveListener(ShowDown);
        searchInput.onEndEdit.RemoveListener(Search);
        
        GuildSearchResultDisplay.OnJoinGuild -= JoinGuild;
    }
    
    private void Search(string _searchName)
    {
        ShowGuilds();
    }

    private void ShowUp()
    {
        Vector3 _itemsPosition = resultsHolder.transform.position;
        _itemsPosition.y -= moveAmount;
        resultsHolder.transform.position = _itemsPosition;
    }

    private void ShowDown()
    {
        Vector3 _itemsPosition = resultsHolder.transform.position;
        _itemsPosition.y += moveAmount;
        resultsHolder.transform.position = _itemsPosition;
    }

    public override void Setup()
    {
        searchInput.text = string.Empty;
        ShowGuilds();
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private void ShowGuilds()
    {
        CleanShownGuilds();
        
        if (DataManager.Instance.GameData.Guilds.Count==0)
        {
            noGuildsMessage.SetActive(true);
            return;
        }
        
        noGuildsMessage.SetActive(false);
        string _searchKey = searchInput.text;

        foreach (var (_key,_value) in DataManager.Instance.GameData.Guilds.ToList().OrderBy(_guild => _guild.Value
        .Name))
        {
            if (!string.IsNullOrEmpty(_searchKey))
            {
                if (!_value.Name.Contains(_searchKey))
                {
                    continue;
                }
            }
            GuildSearchResultDisplay _searchResult = Instantiate(searchResultDisplay, resultsHolder);
            _searchResult.Setup(_value);
            shownObjects.Add(_searchResult.gameObject);
        }

        searchWithNoResultsMessage.gameObject.SetActive(shownObjects.Count==0);
    }

    private void CleanShownGuilds()
    {
        foreach (var _shownObject in shownObjects)
        {
            Destroy(_shownObject);
        }

        shownObjects.Clear();
    }

    private void JoinGuild(GuildData _guildData)
    {
        if (!string.IsNullOrEmpty(DataManager.Instance.PlayerData.GuildId))
        {
            alreadyInGuild.SetActive(true);
            return;
        }
        if (DataManager.Instance.PlayerData.Points<_guildData.MinimumPoints)
        {
            noEnaughtPointsMessage.SetActive(true);
            return;
        }
        joiningGuildPanel.SetActive(true);
        FirebaseManager.Instance.JoinGuild(FirebaseManager.Instance.PlayerId,_guildData.Id,FinishedJoiningGuild);
    }

    private void FinishedJoiningGuild()
    {
        joiningGuildPanel.SetActive(false);
        if (string.IsNullOrEmpty(DataManager.Instance.PlayerData.GuildId))
        {
            ShowGuilds();
            return;
        }
        
        OnJoinedGuild?.Invoke();
    }
}
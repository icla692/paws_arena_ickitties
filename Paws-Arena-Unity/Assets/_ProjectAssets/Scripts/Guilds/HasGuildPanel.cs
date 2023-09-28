using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HasGuildPanel : GuildPanelBase
{
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI leaderNameDisplay;
    [SerializeField] private Image guildBadgeDisplay;
    [SerializeField] private Image kittyIconDisplay;
    [SerializeField] private TextMeshProUGUI membersDisplay;
    [SerializeField] private TextMeshProUGUI winsDisplay;
    [SerializeField] private GuildPlayerDisplay guildPlayerPrefab;
    [SerializeField] private Transform playersHolder;
    [SerializeField] private Button upArrow;
    [SerializeField] private Button downArrow;
    [SerializeField] private Button leaveGuild;
    [SerializeField] private GameObject confirmationForLeaveing;
    [SerializeField] private Button yesLeaveGuild;
    [SerializeField] private GameObject confirmationForKickingPlayer;
    [SerializeField] private Button yesKickPlayer;

    private List<GameObject> shownPlayers = new();
    private float moveAmount = 1;
    private GuildPlayerData kickingPlayer;

    public override void Setup()
    {
        ShowGuildData();
        ClearShownPlayers();
        ShowPlayers();
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        upArrow.onClick.AddListener(MovePlayersUp);
        downArrow.onClick.AddListener(MovePlayersDown);
        GuildPlayerDisplay.OnKickPlayer += KickPlayer;
        leaveGuild.onClick.AddListener(AskForLeaveConfirmation);
        yesLeaveGuild.onClick.AddListener(YesLeaveGuild);
        yesKickPlayer.onClick.AddListener(YesKickPlayer);
    }

    private void OnDisable()
    {
        upArrow.onClick.RemoveListener(MovePlayersUp);
        downArrow.onClick.RemoveListener(MovePlayersDown);
        GuildPlayerDisplay.OnKickPlayer -= KickPlayer;
        leaveGuild.onClick.RemoveListener(AskForLeaveConfirmation);
        yesLeaveGuild.onClick.RemoveListener(YesLeaveGuild);
        yesKickPlayer.onClick.RemoveListener(YesKickPlayer);
    }

    private void ShowGuildData()
    {
        GuildData _guild = DataManager.Instance.PlayerData.Guild;
        GuildSO _guildSO = GuildSO.Get(_guild.FlagId);
        nameDisplay.text = _guild.Name;
        leaderNameDisplay.text = _guild.GetLeader().Name;
        guildBadgeDisplay.sprite = _guildSO.Badge;
        kittyIconDisplay.sprite = _guildSO.Kitty;
        winsDisplay.text = "Matches won:"+_guild.MatchesWon;
        membersDisplay.text = $"Members: {_guild.Players.Count}/{DataManager.Instance.GameData.GuildMaxPlayers}";
    }

    private void ClearShownPlayers()
    {
        foreach (var _player in shownPlayers)
        {
            Destroy(_player);
        }
        
        shownPlayers.Clear();
    }

    private void ShowPlayers()
    {
        GuildPlayerData _leader = DataManager.Instance.PlayerData.Guild.GetLeader();
        bool _showKickOption = FirebaseManager.Instance.PlayerId == _leader.Id;
        foreach (var _player in DataManager.Instance.PlayerData.Guild.Players)
        {
            GuildPlayerDisplay _playerDisplay = Instantiate(guildPlayerPrefab, playersHolder);
            _playerDisplay.Setup(_player,_showKickOption);
            shownPlayers.Add(_playerDisplay.gameObject);
        }
    }

    private void MovePlayersUp()
    {
        Vector3 _itemsPosition = playersHolder.transform.position;
        _itemsPosition.y -= moveAmount;
        playersHolder.transform.position = _itemsPosition;
    }

    private void MovePlayersDown()
    {
        Vector3 _itemsPosition = playersHolder.transform.position;
        _itemsPosition.y += moveAmount;
        playersHolder.transform.position = _itemsPosition;
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private void KickPlayer(GuildPlayerData _player)
    {
        kickingPlayer = _player;
        confirmationForKickingPlayer.SetActive(true);
    }
    
    private void YesKickPlayer()
    {
        confirmationForKickingPlayer.SetActive(false);
        FirebaseManager.Instance.RemovePlayerFromGuild(kickingPlayer.Id, DataManager.Instance.PlayerData.GuildId);
        Setup();
    }

    private void AskForLeaveConfirmation()
    {
        confirmationForLeaveing.SetActive(true);
    }

    private void YesLeaveGuild()
    {
        string _guildId = DataManager.Instance.PlayerData.GuildId;
        bool _amILeader = false;
        foreach (var _player in DataManager.Instance.GameData.Guilds[_guildId].Players)
        {
            if (_player.IsLeader)
            {
                if (_player.Id==FirebaseManager.Instance.PlayerId)
                {
                    _amILeader = true;
                }
                break;
            }
        }
        bool _deleteGuild = DataManager.Instance.GameData.Guilds[_guildId].Players.Count == 1;
        if (_deleteGuild)
        {
            FirebaseManager.Instance.DeleteGuild();
        }
        else
        {
            if (_amILeader)
            {
                foreach (var _player in DataManager.Instance.GameData.Guilds[_guildId].Players)
                {
                    if (!_player.IsLeader)
                    {
                        FirebaseManager.Instance.SetNewGuildLeader(_player.Id,_guildId, RemovePlayer);
                        break;
                    }
                }
            }
            else
            {
                RemovePlayer();
            }
            
        }
        
        DataManager.Instance.PlayerData.GuildId = string.Empty;
        confirmationForLeaveing.SetActive(false);

        void RemovePlayer()
        {
            FirebaseManager.Instance.RemovePlayerFromGuild(FirebaseManager.Instance.PlayerId, _guildId);
        }
    }
}

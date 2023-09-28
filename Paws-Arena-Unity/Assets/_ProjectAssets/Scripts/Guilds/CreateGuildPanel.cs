using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateGuildPanel : GuildPanelBase
{
    public static Action<GuildSO> OnGuildSelected;

    [SerializeField] private GuildKittyDisplay guildKittyPrefab;
    [SerializeField] private Transform guildKittyHolder;
    [SerializeField] private Image badgeDisplay;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField minPointsInput;
    [SerializeField] private Button createButton;
    [SerializeField] private GameObject noEnaughtSnacks;
    [SerializeField] private TextMeshProUGUI noEnaughtSnacksMessage;
    [SerializeField] private GameObject invalidName;
    [SerializeField] private TextMeshProUGUI invalidNameMessage;
    [SerializeField] private GameObject nameTaken;

    private List<GameObject> shownKitties = new();
    private GuildSO selectedGuild;

    public override void Setup()
    {
        ClearKitties();
        ShowKitties();
        minPointsInput.text = 0.ToString();
        gameObject.SetActive(true);
    }

    private void Start()
    {
        SelectGuild(GuildSO.Get(0));
    }

    private void ClearKitties()
    {
        foreach (var _kitty in shownKitties)
        {
            Destroy(_kitty);
        }

        shownKitties.Clear();
    }

    private void ShowKitties()
    {
        foreach (var _guildKittySO in GuildSO.GetAll())
        {
            GuildKittyDisplay _kittyDisplay = Instantiate(guildKittyPrefab, guildKittyHolder);
            _kittyDisplay.Setup(_guildKittySO);
            shownKitties.Add(_kittyDisplay.gameObject);
        }
    }

    private void OnEnable()
    {
        previousButton.onClick.AddListener(ShowPrevious);
        nextButton.onClick.AddListener(ShowNext);
        createButton.onClick.AddListener(ValidateGuild);

        GuildKittyDisplay.OnGuildSelected += SelectGuild;
    }

    private void OnDisable()
    {
        previousButton.onClick.RemoveListener(ShowPrevious);
        nextButton.onClick.RemoveListener(ShowNext);
        createButton.onClick.RemoveListener(ValidateGuild);
        
        GuildKittyDisplay.OnGuildSelected -= SelectGuild;
    }

    private void ShowPrevious()
    {
        GuildSO _guild = GuildSO.Get(selectedGuild.Id-1);
        if (_guild==null)
        {
            return;
        }
        
        SelectGuild(_guild);
    }
    

    private void ShowNext()
    {
        GuildSO _guild = GuildSO.Get(selectedGuild.Id+1);
        if (_guild==null)
        {
            return;
        }
        
        SelectGuild(_guild);
    }

    private void SelectGuild(GuildSO _guildKitty)
    {
        selectedGuild = _guildKitty;
        badgeDisplay.sprite = selectedGuild.Badge;
        OnGuildSelected?.Invoke(_guildKitty);
    }

    private void ValidateGuild()
    {
        if (DataManager.Instance.PlayerData.Snacks < DataManager.Instance.GameData.GuildPrice)
        {
            noEnaughtSnacksMessage.text =
                $"You dont have enaught cookies! You need {DataManager.Instance.GameData.GuildPrice} cookies to create a Guild";
            noEnaughtSnacks.SetActive(true);
            return;
        }

        string _name = nameInput.text;

        if (!ValidateName(_name))
        {
            return;
        }

        FirebaseManager.Instance.ValidateGuildName(_name, CreateGuild, ShowNameTaken);
    }

    private void ShowNameTaken()
    {
        nameTaken.SetActive(true);
    }

    private void CreateGuild()
    {
        int _minPoints = Convert.ToInt32(minPointsInput.text);

        GuildData _newGuild = new();
        _newGuild.Name = nameInput.text;
        _newGuild.Id = Guid.NewGuid().ToString();
        _newGuild.Players = new();
        _newGuild.Players.Add(new GuildPlayerData()
        {
            Id = DataManager.Instance.PlayerData.PlayerId,
            Name = GameState.nickname,
            IsLeader = true,
            Level = DataManager.Instance.PlayerData.Level,
            Points = 0
        });
        _newGuild.FlagId = selectedGuild.Id;
        _newGuild.MinimumPoints = _minPoints;
        
        DataManager.Instance.GameData.Guilds.Add(_newGuild.Id,_newGuild);
        FirebaseManager.Instance.CreateGuild(_newGuild);
        DataManager.Instance.PlayerData.GuildId = _newGuild.Id;
        DataManager.Instance.PlayerData.Snacks -= DataManager.Instance.GameData.GuildPrice;
    }

    private bool ValidateName(string _name)
    {
        int _minLenght = 3;
        int _maxlenght=15;
        if (_name.Length<_minLenght||_name.Length>_maxlenght)
        {
            invalidNameMessage.text = $"Name must contain minimum {_minLenght} characters and maximum {_maxlenght}";
            invalidName.SetActive(true);
            return false;
        }

        return true;
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}

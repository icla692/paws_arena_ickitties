using System;
using UnityEngine;
using UnityEngine.UI;

public class GuildLeftPanel : MonoBehaviour
{
    public static Action OnShowMyGuild;
    public static Action OnShowGuildBattle;
    public static Action OnShowSearchGuilds;
    public static Action OnClose;
    public static Action OnShowTopGuilds;
    
    [SerializeField] private Image flagImage;
    [SerializeField] private Button myGuildButton;
    [SerializeField] private Button guildBattleButton;
    [SerializeField] private Button topGuilds;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button searchGuilds;
    [Space]
    [SerializeField] private Sprite selectedButton;
    [SerializeField] private Sprite notSelectedButton;
    
    [SerializeField] private GameObject noGuildMessage;


    private void OnEnable()
    {
        DataManager.Instance.PlayerData.UpdatedGuild += UpdateFlag;
        
        closeButton.onClick.AddListener(Close);
        myGuildButton.onClick.AddListener(ShowMyGuild);
        guildBattleButton.onClick.AddListener(ShowGuildBattle);
        topGuilds.onClick.AddListener(ShowTopGuilds);
        searchGuilds.onClick.AddListener(ShowSearchGuilds);

        Invoke(nameof(ShowMyGuild),0.1f);
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.UpdatedGuild -= UpdateFlag;
        
        closeButton.onClick.RemoveListener(Close);
        myGuildButton.onClick.RemoveListener(ShowMyGuild);
        guildBattleButton.onClick.RemoveListener(ShowGuildBattle);
        topGuilds.onClick.RemoveListener(ShowTopGuilds);
        searchGuilds.onClick.RemoveListener(ShowSearchGuilds);
    }

    private void UpdateFlag()
    {
        Invoke(nameof(ShowFlag),1f);
    }

    private void Close()
    {
        OnClose?.Invoke();
    }

    private void ShowFlag()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            flagImage.gameObject.SetActive(false);
            return;
        }
        
        flagImage.gameObject.SetActive(true);
    }

    private void ShowMyGuild()
    {
        ShowButtonAsSelected(myGuildButton);
        ShowFlag();
        OnShowMyGuild?.Invoke();
    }

    private void ShowGuildBattle()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            noGuildMessage.SetActive(true);
            return;
        }
        ShowButtonAsSelected(guildBattleButton);
        OnShowGuildBattle?.Invoke();
    }

    private void ShowButtonAsSelected(Button _button)
    {
        myGuildButton.image.sprite = notSelectedButton;
        guildBattleButton.image.sprite = notSelectedButton;
        topGuilds.image.sprite = notSelectedButton;
        searchGuilds.image.sprite = notSelectedButton;

        _button.image.sprite = selectedButton;
    }

    private void ShowTopGuilds()
    {
        ShowButtonAsSelected(topGuilds);
        OnShowTopGuilds?.Invoke();
    }
    
    private void ShowSearchGuilds()
    {
        ShowButtonAsSelected(searchGuilds);
        OnShowSearchGuilds?.Invoke();
    }
}

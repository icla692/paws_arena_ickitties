using System;
using UnityEngine;
using UnityEngine.UI;

public class NoGuildPanel : GuildPanelBase
{
    public static Action OnShowCreateGuild;
    public static Action OnShowJoinGuild;
    
    [SerializeField] private Button createGuildButton;
    [SerializeField] private Button joinGuildButton;
    
    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        createGuildButton.onClick.AddListener(ShowCreateGuildPanel);
        joinGuildButton.onClick.AddListener(ShowJoinGuildPanel);
    }

    private void OnDisable()
    {
        createGuildButton.onClick.RemoveListener(ShowCreateGuildPanel);
        joinGuildButton.onClick.RemoveListener(ShowJoinGuildPanel);
    }

    private void ShowCreateGuildPanel()
    {
        OnShowCreateGuild?.Invoke();
    }

    private void ShowJoinGuildPanel()
    {
        OnShowJoinGuild?.Invoke();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}

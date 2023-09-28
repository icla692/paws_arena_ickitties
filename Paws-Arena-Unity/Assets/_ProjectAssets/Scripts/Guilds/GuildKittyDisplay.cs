using System;
using UnityEngine;
using UnityEngine.UI;

public class GuildKittyDisplay : MonoBehaviour
{
    public static Action<GuildSO> OnGuildSelected;
    [SerializeField] private Button selectGuild;
    [SerializeField] private Image kittySelected;
    [SerializeField] private Image kittyDisplay;

    private GuildSO guild;
    
    public void Setup(GuildSO _guild)
    {
        guild = _guild;

        kittyDisplay.sprite = _guild.Kitty;
        kittySelected.sprite = _guild.SelectedKitty;
    }

    private void OnEnable()
    {
        selectGuild.onClick.AddListener(Select);
        CreateGuildPanel.OnGuildSelected += ShowSelected;
    }

    private void OnDisable()
    {
        selectGuild.onClick.RemoveListener(Select);
        CreateGuildPanel.OnGuildSelected -= ShowSelected;
    }

    private void ShowSelected(GuildSO _selectedGuild)
    {
        kittySelected.gameObject.SetActive(_selectedGuild.Id==guild.Id);
    }

    private void Select()
    {
        OnGuildSelected?.Invoke(guild);
    }
}

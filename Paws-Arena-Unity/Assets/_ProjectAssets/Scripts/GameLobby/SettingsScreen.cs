using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
    public LobbyUIManager uiManager;
    public CustomSlider masterVolume;
    public CustomSlider musicVolume;
    public CustomSlider sfxVolume;

    private void OnEnable()
    {
        ShowSettings(GameState.gameSettings);
    }

    public void Apply()
    {
        GameState.gameSettings.masterVolume = masterVolume.GetValue();
        GameState.gameSettings.musicVolume = musicVolume.GetValue();
        GameState.gameSettings.soundFXVolume = sfxVolume.GetValue();
        GameState.gameSettings.Apply();

        uiManager.CloseSettings();
    }
    private void ShowSettings(GameSettings gameSettings)
    {
        masterVolume.SetValue(gameSettings.masterVolume);
        musicVolume.SetValue(gameSettings.musicVolume);
        sfxVolume.SetValue(gameSettings.soundFXVolume);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public static event Action onGameSettingsApplied;
    private const string key = "gamesettings";

    [SerializeField]
    public float masterVolume = 1f;
    [SerializeField]
    public float musicVolume = 1f;
    [SerializeField]
    public float soundFXVolume = 1f;

    public static GameSettings Default()
    {
        if (PlayerPrefs.HasKey(key))
        {
            return JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString(key));
        }
        return new GameSettings()
        {
            masterVolume = 1f,
            musicVolume = 1f,
            soundFXVolume = 1f
        };
    }
    public void Apply()
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(this));
        onGameSettingsApplied?.Invoke();
    }
}

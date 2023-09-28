using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoSingleton<SFXManager>
{
    public AudioSource musicSource;
    public AudioSource oneShotAudioSource;

    private float initialMusicSourceVolume;
    private float initialOneShotSourceVolume;
    private float oneShotSourceBaseVolume;

    public IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        if (oneShotAudioSource != null)
        {
            initialOneShotSourceVolume = oneShotAudioSource.volume;
        }

        if(musicSource != null)
        {
            initialMusicSourceVolume = musicSource.volume;
        }

        ApplySettings();
    }

    private void OnEnable()
    {
        GameSettings.onGameSettingsApplied += ApplySettings;
    }

    private void OnDisable()
    {
        GameSettings.onGameSettingsApplied -= ApplySettings;
    }

    private void ApplySettings()
    {
        if(musicSource != null)
        {
            musicSource.volume = GameState.gameSettings.musicVolume * GameState.gameSettings.masterVolume;
        }

        if(oneShotAudioSource != null)
        {
            oneShotSourceBaseVolume = oneShotAudioSource.volume = GameState.gameSettings.soundFXVolume * GameState.gameSettings.masterVolume;
        }
    }

    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        StopOneShot();
        oneShotAudioSource.volume = oneShotAudioSource.volume > 0 ? volume * oneShotSourceBaseVolume : 0;
        oneShotAudioSource.PlayOneShot(clip);
    }

    public void StopOneShot()
    {
        oneShotAudioSource.Stop();
    }
}

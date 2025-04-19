using System;
using UnityEngine;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    [SerializeField] AudioMixer sfx, bgm; // AudioMixer for SFX and BGM
    public static event Action OnVolumeChanged; // Action trigger to detect when sound was changed

    // Load the volume settings on awake
    void Awake()
    {   
        LoadVolumeSettings();
    }

    // Set the BGM volume
    public void SetBGMVolume(float volume)
    {
        float db = ConvertToDecibels(volume);
        bgm.SetFloat("BGM_Volume", db);
        PlayerPrefs.SetFloat("BGM_Volume", volume);
        OnVolumeChanged?.Invoke();
    }

    // Set the SFX volume
    public void SetSFXVolume(float volume)
    {
        float db = ConvertToDecibels(volume);
        sfx.SetFloat("SFX_Volume", db);
        PlayerPrefs.SetFloat("SFX_Volume", volume);
        OnVolumeChanged?.Invoke();
    }

    // Load the volume settings from playerprefs (or default, if none exists)
    void LoadVolumeSettings()
    {
        SetBGMVolume(PlayerPrefs.GetFloat("BGM_Volume", 0.4f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFX_Volume", 0.6f));
    }

    // Converts [0-1] value to logarithmic scale value (decibel)
    float ConvertToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
    }
    
}

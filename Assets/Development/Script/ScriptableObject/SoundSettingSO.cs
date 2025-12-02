using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "Settings/Sound Volume")]
public class SoundSettingSO : ScriptableObject
{
    [Header("Sound Settings")]
    [Range(0f, 1f)]
    public float masterVolume = 1.0f;

    [Range(0f, 1f)]
    public float bgmVolume = 1.0f;

    [Range(0f, 1f)]
    public float sfxVolume = 1.0f;

    [Range(0f, 1f)]
    public float uiVolume = 1.0f;

    // Public properties with calculated final volume
    public float MasterVolume => masterVolume;
    public float BGMVolume => bgmVolume * masterVolume;
    public float SFXVolume => sfxVolume * masterVolume;
    public float UIVolume => uiVolume * masterVolume;

    // Setter methods for runtime adjustment
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetUIVolume(float volume)
    {
        uiVolume = Mathf.Clamp01(volume);
    }
}
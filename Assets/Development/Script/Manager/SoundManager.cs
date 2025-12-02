using UnityEngine;

public class _SoundManager : Singleton<_SoundManager>
{
    [Header("ScriptableObject References")]
    [SerializeField] private SoundSettingSO soundSettings;

    // Public static properties for easy access
    public static SoundSettingSO Settings => Instance.soundSettings;

    public static float MasterVolume => Settings.MasterVolume;
    public static float BGMVolume => Settings.BGMVolume;
    public static float SFXVolume => Settings.SFXVolume;
    public static float UIVolume => Settings.UIVolume;

    protected override void Awake()
    {
        base.Awake();

        if (soundSettings == null)
        {
            soundSettings = Resources.Load<SoundSettingSO>("SoundSettings");
        }
    }

    // ========== Static Setter Methods ==========
    public static void SetMasterVolume(float volume) => Settings.SetMasterVolume(volume);
    public static void SetBGMVolume(float volume) => Settings.SetBGMVolume(volume);
    public static void SetSFXVolume(float volume) => Settings.SetSFXVolume(volume);
    public static void SetUIVolume(float volume) => Settings.SetUIVolume(volume);
}

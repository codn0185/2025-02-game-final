using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "Settings/Sound Volume")]
public class SoundSettingSO : ScriptableObject
{
    private static SoundSettingSO _instance;
    public static SoundSettingSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<SoundSettingSO>("SoundSettings");
                if (_instance == null)
                {
                    Debug.LogWarning("SoundSettings not found in Resources folder. Using default values.");
                    _instance = CreateInstance<SoundSettingSO>();
                }
            }
            return _instance;
        }
    }

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1.0f;

    [Range(0f, 1f)]
    [SerializeField] private float bgmVolume = 1.0f;

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1.0f;

    [Range(0f, 1f)]
    [SerializeField] private float uiVolume = 1.0f;

    // Public properties with calculated final volume
    public static float MasterVolume => Instance.masterVolume;
    public static float BGMVolume => Instance.bgmVolume * Instance.masterVolume;
    public static float SFXVolume => Instance.sfxVolume * Instance.masterVolume;
    public static float UIVolume => Instance.uiVolume * Instance.masterVolume;

    // Setter methods for runtime adjustment
    public static void SetMasterVolume(float volume)
    {
        Instance.masterVolume = Mathf.Clamp01(volume);
    }

    public static void SetBGMVolume(float volume)
    {
        Instance.bgmVolume = Mathf.Clamp01(volume);
    }

    public static void SetSFXVolume(float volume)
    {
        Instance.sfxVolume = Mathf.Clamp01(volume);
    }

    public static void SetUIVolume(float volume)
    {
        Instance.uiVolume = Mathf.Clamp01(volume);
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "VolumeRatios", menuName = "Settings/Volume Ratios")]
public class VolumeRatioSO : ScriptableObject
{
    private static VolumeRatioSO _instance;
    public static VolumeRatioSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<VolumeRatioSO>("VolumeRatios");
                if (_instance == null)
                {
                    Debug.LogWarning("VolumeRatios not found in Resources folder. Using default ratios.");
                    _instance = CreateInstance<VolumeRatioSO>();
                }
            }
            return _instance;
        }
    }

    [Header("BGM Ratios")]
    [Range(0f, 1f)] public float bgmMainTheme = 1.0f;
    [Range(0f, 1f)] public float bgmBattleTheme = 1.0f;

    [Header("SFX - Player Ratios")]
    [Range(0f, 1f)] public float sfxPlayerAttack = 0.5f;
    [Range(0f, 1f)] public float sfxPlayerHit = 0.5f;
    [Range(0f, 1f)] public float sfxPlayerDie = 0.5f;
    [Range(0f, 1f)] public float sfxPlayerLevelUp = 0.5f;

    [Header("SFX - Monster Ratios")]
    [Range(0f, 1f)] public float sfxMonsterSpawn = 0.5f;
    [Range(0f, 1f)] public float sfxMonsterAttack = 0.5f;
    [Range(0f, 1f)] public float sfxMonsterHit = 0.5f;
    [Range(0f, 1f)] public float sfxMonsterDie = 0.5f;

    [Header("SFX - Item Ratios")]
    [Range(0f, 1f)] public float sfxItemDrop = 0.5f;
    [Range(0f, 1f)] public float sfxItemCollect = 0.5f;

    [Header("UI Ratios")]
    [Range(0f, 1f)] public float uiButtonClick = 0.5f;
    [Range(0f, 1f)] public float uiNotification = 0.5f;
}

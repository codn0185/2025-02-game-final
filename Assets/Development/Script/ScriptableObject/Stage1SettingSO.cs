using UnityEngine;

[CreateAssetMenu(fileName = "StageSettings", menuName = "Stage setting")]
public class Stage1SettingSO : ScriptableObject
{
    [Header("Monsters")]
    public GameObject[] enemies;

    public int[] totalEnemies = { 50, 70, 100 };
    public int[] enemyRatio1 = { 100, 0, 0 };
    public int[] enemyRatio2 = { 60, 30, 10 };
    public int[] enemyRatio3 = { 40, 40, 20 };

    // Setter methods for runtime adjustment
    public void SetMasterVolume(float volume)
    {
        // masterVolume = Mathf.Clamp01(volume);
    }

    public void SetBGMVolume(float volume)
    {
        // bgmVolume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        // sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetUIVolume(float volume)
    {
        // uiVolume = Mathf.Clamp01(volume);
    }
}
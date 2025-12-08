using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // =========== UI References ==========
    [Header("Main UI Panels")]
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject GameClearUI;

    // =========== In-Game UI Elements ==========
    [Header("In-Game UI Elements")]
    [SerializeField] private Slider InGamePlayerHPSlider;
    [SerializeField] private Text InGameKillCountText;
    [SerializeField] private Text InGamePlayTimeText;
    [SerializeField] private Text InGameStageRoundText;
    [SerializeField] private Text InGameRoundLeftKillCountText;


    // ========== Unity Lifecycle ==========
    protected override void Awake()
    {
        base.Awake();
    }

    // ========== UI Show/Hide Methods ==========
    public void ShowPauseUI()
    {
        PauseUI.SetActive(true);
    }

    public void HidePauseUI()
    {
        PauseUI.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        GameOverUI.SetActive(true);
    }

    public void HideGameOverUI()
    {
        GameOverUI.SetActive(false);
    }

    public void ShowGameClearUI()
    {
        GameClearUI.SetActive(true);
    }

    public void HideGameClearUI()
    {
        GameClearUI.SetActive(false);
    }

    // ========= In-Game UI Update Methods ==========

    public void UpdatePlayerHP(float currentHP, float maxHP)
    {
        InGamePlayerHPSlider.maxValue = maxHP;
        InGamePlayerHPSlider.value = currentHP;
    }

    public void UpdatePlayerHP()
    {
        UpdatePlayerHP(GameProgressManager.Instance.PlayerCurrentHealth, GameProgressManager.Instance.PlayerMaxHealth);
    }

    public void UpdateKillCount(int killCount)
    {
        InGameKillCountText.text = $"Kills: {killCount}";
    }

    public void UpdateKillCount()
    {
        UpdateKillCount(GameProgressManager.Instance.PlayerTotalKillCount);
    }

    public void UpdatePlayTime(float playTime)
    {
        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        InGamePlayTimeText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }

    public void UpdatePlayTime()
    {
        UpdatePlayTime(GameProgressManager.Instance.PlayTime);
    }

    public void UpdateStageRound(int stage, int round)
    {
        InGameStageRoundText.text = $"Stage: {stage} - Round: {round}";
    }

    public void UpdateStageRound()
    {
        UpdateStageRound(GameProgressManager.Instance.CurrentStage, GameProgressManager.Instance.CurrentRound);
    }

    public void UpdateRoundLeftKillCount(int leftKillCount)
    {
        InGameRoundLeftKillCountText.text = $"Left Kills: {leftKillCount}";
    }

    public void UpdateRoundLeftKillCount()
    {
        UpdateRoundLeftKillCount(SpawnManager.Instance.RoundLeftKillCount);
    }
}
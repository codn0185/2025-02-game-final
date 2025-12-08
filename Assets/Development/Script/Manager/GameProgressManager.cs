/* 
게임 진행 관리 매니저
 - 로그라이크 게임의 진행 상황(스테이지 및 라운드)을 관리하는 싱글톤 매니저 클래스
 - 게임 오버 시 현재 진행 중인 스테이지는 초기화되고, 최고 기록만 저장
 - 각 플레이는 독립적이며, 게임 오버 시 처음부터 다시 시작
 */

using System;
using UnityEngine;

public enum GameProgressState
{
    Idle,           // 대기 상태 (메뉴, 로비)
    Playing,        // 플레이 중
    Paused,         // 일시정지
    GameOver,       // 게임 오버
    GameClear       // 게임 클리어
}

public class GameProgressManager : Singleton<GameProgressManager>
{
    // ========== 현재 진행 상태 ==========
    private GameProgressState currentState = GameProgressState.Idle;
    [SerializeField] private int currentStage = 0;
    [SerializeField] private int currentRound = 0;

    // ========== 속성 ==========
    public GameProgressState CurrentState => currentState;
    public int CurrentStage => currentStage;
    public int CurrentRound => currentRound;

    // ========== Unity Lifecycle ==========
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        ChangeState(GameProgressState.Playing);
    }

    // ========== 플레이어 정보 관리 ==========
    public void AddExperience(int amount)
    {
        // 경험치 추가 로직 구현
    }

    public void AddCoins(int amount)
    {
        // 코인 추가 로직 구현
    }
    
    public void AddGems(int amount)
    {
        // 보석 추가 로직 구현
    }

    // ========== 스테이지 관리 ==========
    public void StartStage(int stage)
    {
        currentStage = stage;
        currentRound = 0;
        ChangeState(GameProgressState.Playing);
    }

    public void CompleteStage()
    {
        SaveProgress();
    }

    public void NextStage()
    {
        StartStage(currentStage + 1);
    }

    // ========== 라운드 관리 ==========
    public void StartRound(int round)
    {
        currentRound = round;
        ChangeState(GameProgressState.Playing);
    }

    public void CompleteRound()
    {
        SaveProgress();
    }

    public void NextRound()
    {
        StartRound(currentRound + 1);
    }

    // ========== 게임 상태 관리 ==========
    public void ChangeState(GameProgressState newState)
    {
        if (currentState == newState) return;

        GameProgressState oldState = currentState;
        currentState = newState;

        HandleStateTransition(oldState, newState);
    }

    private void HandleStateTransition(GameProgressState oldState, GameProgressState newState)
    {
        // 이전 상태 종료 처리
        switch (oldState)
        {
            case GameProgressState.Idle:
                break;
            case GameProgressState.Playing:
                break;
            case GameProgressState.Paused:
                Time.timeScale = 1f;
                HidePauseUI(); // 일시정지 UI 숨기기
                break;
            case GameProgressState.GameOver:
                HideGameOverUI(); // 게임 오버 UI 숨기기
                break;
            case GameProgressState.GameClear:
                HideGameClearUI(); // 게임 클리어 UI 숨기기
                break;
        }

        // 새 상태 시작 처리
        switch (newState)
        {
            case GameProgressState.Idle:
                Time.timeScale = 1f;
                break;
            case GameProgressState.Playing:
                Time.timeScale = 1f;
                BackgroundManager.Instance.StartSpawningBackgrounds();
                break;
            case GameProgressState.Paused:
                Time.timeScale = 0f;
                ShowPauseUI();
                break;
            case GameProgressState.GameOver:
                Time.timeScale = 0f;
                SaveMaxProgressToProfile();  // 프로필에 최고 기록 저장
                ShowGameOverUI(); // 게임 오버 UI 표시
                ResetProgress(); // 진행 상황 초기화
                BackgroundManager.Instance.StopSpawningBackgrounds(); // 배경 소환 중지

                break;
            case GameProgressState.GameClear:
                Time.timeScale = 0f;
                SaveMaxProgressToProfile();  // 프로필에 최고 기록 저장
                ShowGameClearUI(); // 게임 클리어 UI 표시
                ResetProgress(); // 진행 상황 초기화
                BackgroundManager.Instance.StopSpawningBackgrounds(); // 배경 소환 중지

                break;
        }
    }

    public void PauseGame()
    {
        ChangeState(GameProgressState.Paused);
    }

    public void ResumeGame()
    {
        ChangeState(GameProgressState.Playing);
    }

    public void GameOver()
    {
        ChangeState(GameProgressState.GameOver);
    }

    public void GameClear()
    {
        ChangeState(GameProgressState.GameClear);
    }

    public void ExitGame()
    {
        ChangeState(GameProgressState.Idle);
    }

    // ========== 진행 상황 저장/로드 ==========
    private void SaveProgress()
    {
        if (ProfileManager.Instance.CurrentState == ProfileState.Active)
        {
            ProfileManager.Instance.GameData.UpdateMaxProgress(currentStage, currentRound);
            ProfileManager.Instance.SaveActiveProfile();
        }
    }

    // 프로필에 최고 진행 사항 저장
    private void SaveMaxProgressToProfile()
    {
        if (ProfileManager.Instance.CurrentState == ProfileState.Active)
        {
            ProfileManager.Instance.GameData.UpdateMaxProgress(currentStage, currentRound);
            ProfileManager.Instance.SaveActiveProfile();
        }
    }

    // 프로필 최고 진행 사항 불러오기
    public void LoadProgress()
    {
        if (ProfileManager.Instance.CurrentState == ProfileState.Active)
        {
            currentStage = ProfileManager.Instance.GameData.MaxStage;
            currentRound = ProfileManager.Instance.GameData.MaxRound;
        }
    }

    // 진행 상황 초기화
    public void ResetProgress()
    {
        currentStage = 0;
        currentRound = 0;
    }

    // ========== UI 관리 ==========
    protected void ShowPauseUI()
    {
        //
    }

    protected void HidePauseUI()
    {
        //
    }

    protected void ShowGameOverUI()
    {
        //
    }

    protected void HideGameOverUI()
    {
        //
    }

    protected void ShowGameClearUI()
    {
        //
    }

    protected void HideGameClearUI()
    {
        //
    }
}
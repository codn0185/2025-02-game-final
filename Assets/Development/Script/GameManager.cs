using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MAIN_MENU,
    GAME_PLAY,
    GAME_PAUSE,
    GAME_OVER,
    GAME_CLEAR
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState CurrentState { get; private set; }
    private const float STATE_CHANGE_DELAY = 0.2f;

    public GameObject mainMenuUI;
    public GameObject inGameUI;
    public GameObject gamePauseUI;
    public GameObject gameOverUI;
    public GameObject gameClearUI;


    // 공격 관련
    public const int MAX_BULLET_DAMAGE = 100;
    public const int MAX_BULLET_COUNT = 15;
    public const float MIN_ATTACK_SPEED = 0.05f;
    // How many times player attacks per 5 seconds
    public int attack_speed = 5;
    public int attack_type;
    public int bullet_damage;
    public int bullet_count;
    public int bullet_hit_count = 1;

    // 플레이 데이터
    float play_time = 0f;
    int max_player_hp = 10;
    int player_hp = 0;
    int kill_count = 0;
    public Round.RoundData CurrentRoundData { get; private set; }
    int game_round = 0;
    float round_time = 0f;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        ChangeState(GameState.MAIN_MENU); ;
    }

    void Update()
    {
        if (CurrentState != GameState.GAME_PLAY)
        {
            return;
        }

        play_time += Time.deltaTime;
        UIManager.instance.UpdateTime(play_time);

        round_time += Time.deltaTime;
        if (game_round >= 0 && game_round < Round.data.Count)
        {
            float remaining = Round.data[game_round].round_time - round_time;
            UIManager.instance.UpdateRoundTimeText(remaining);
        }

        if (round_time >= Round.data[game_round].round_time)
        {
            AdvanceRound();
        }
    }

    private void ChangeState(GameState newState)
    {
        StartCoroutine(TransitionToState(newState));
    }

    IEnumerator TransitionToState(GameState newState)
    {
        yield return new WaitForSecondsRealtime(STATE_CHANGE_DELAY);
        CurrentState = newState;
        HandleStateChange();
    }

    private void HandleStateChange()
    {
        HideAllMenu();
        switch (CurrentState)
        {
            case GameState.MAIN_MENU:
                Debug.Log("GameState.MAIN_MENU");
                Time.timeScale = 0f;
                mainMenuUI.SetActive(true);

                FillUpHP();
                break;
            case GameState.GAME_PLAY:
                Debug.Log("GameState.GAME_PLAY");
                inGameUI.SetActive(true);
                Time.timeScale = 1f;
                break;
            case GameState.GAME_PAUSE:
                Debug.Log("GameState.GAME_PAUSE");
                gamePauseUI.SetActive(true);
                Time.timeScale = 0f;
                break;
            case GameState.GAME_OVER:
                Debug.Log("GameState.GAME_OVER");
                gameOverUI.SetActive(true);
                Time.timeScale = 0f;
                UIManager.instance.SetGameOverUI(game_round, (int)play_time, kill_count);
                break;
            case GameState.GAME_CLEAR:
                Debug.Log("GameState.GAME_CLEAR");
                gameClearUI.SetActive(true);
                Time.timeScale = 0f;
                UIManager.instance.SetGameClearUI(game_round, (int)play_time, kill_count);
                break;
        }
    }

    public void HideAllMenu()
    {
        mainMenuUI.SetActive(false);
        inGameUI.SetActive(false);
        gamePauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameClearUI.SetActive(false);
    }

    public void ChangeToMainMenu()
    {
        // ChangeState(GameState.MAIN_MENU);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeToPlaying()
    {
        ChangeState(GameState.GAME_PLAY);
    }

    public void ChangeToPause()
    {
        ChangeState(GameState.GAME_PAUSE);
    }

    public void ChangeToGameOver()
    {
        ChangeState(GameState.GAME_OVER);
    }

    public void ChangeToGameClear()
    {
        ChangeState(GameState.GAME_CLEAR);
    }

    void AdvanceRound()
    {
        round_time = 0f;
        game_round++;

        if (game_round > Round.MAX_ROUND)
        {
            game_round = Round.MAX_ROUND;
            ChangeToGameClear();
            return;
        }

        FillUpHP();
        CurrentRoundData = Round.data[game_round];
        UIManager.instance.UpdateRoundText(game_round, Round.MAX_ROUND);
    }

    public bool GetItem(string tag)
    {
        switch (tag)
        {
            case ItemManager.ATK_Speed.tag:
                // attack_speed *= 0.9f;
                // if (attack_speed < MIN_ATTACK_SPEED)
                // {
                //     attack_speed = MIN_ATTACK_SPEED;
                // }
                return true;
            case ItemManager.ATK_Count.tag:
                if (bullet_count < MAX_BULLET_COUNT)
                {
                    bullet_count++;
                }
                return true;
            case ItemManager.ATK_Penetration.tag:
                bullet_hit_count++;
                return true;
            case ItemManager.ATK_Damage.tag:
                bullet_damage++;
                return true;
        }
        return false;
    }

    public void DecreaseHP(int value = 1)
    {
        player_hp--;
        UIManager.instance.UpdateHP(player_hp, max_player_hp);
        if (player_hp == 0)
        {
            ChangeToGameOver();
        }
    }

    public void IncreaseHP(int value = 1)
    {
        player_hp = Math.Min(player_hp + value, max_player_hp);
        UIManager.instance.UpdateHP(player_hp, max_player_hp);
    }

    public void FillUpHP()
    {
        player_hp = max_player_hp;
        UIManager.instance.UpdateHP(player_hp, max_player_hp);
    }

    public void IncreaseKillCount(int count = 1)
    {
        kill_count += count;
        UIManager.instance.UpdateKillCount(kill_count);
    }
}

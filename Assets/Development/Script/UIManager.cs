using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // In Game UI
    public Text InGame_killCountText;
    public Text InGame_playerHPText;
    public Slider InGame_playerHPSlider;
    public Text InGame_playTimeText;
    public Text InGame_roundText;
    public Text InGame_roundTimeText;

    // Pause UI

    // Game Over UI
    public Text GameOverUI_roundText;
    public Text GameOverUI_timeText;
    public Text GameOverUI_killCountText;

    // Game Clear UI

    public Text GameClearUI_roundText;
    public Text GameClearUI_timeText;
    public Text GameClearUI_killCountText;


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
    }

    void Update()
    {
    }


    public void UpdateTime(float play_time)
    {
        if (InGame_playTimeText == null) return;
        int s = (int)play_time % 60;
        int m = (int)play_time / 60;
        InGame_playTimeText.text = string.Format("Time: {0:D2}:{1:D2}", m, s);
    }

    public void UpdateKillCount(int kill_count)
    {
        if (InGame_killCountText == null) return;
        InGame_killCountText.text = string.Format("Kill: {0}", kill_count);
    }

    public void UpdateHP(int hp, int max_hp)
    {
        if (InGame_playerHPText != null)
        {
            InGame_playerHPText.text = string.Format("HP: {0} / {1}", hp, max_hp);
        }
        if (InGame_playerHPSlider != null)
        {
            InGame_playerHPSlider.maxValue = max_hp;
            InGame_playerHPSlider.value = hp;
        }
    }

    public void UpdateRoundText(int round, int max_round)
    {
        if (InGame_roundText == null) return;
        InGame_roundText.text = string.Format("Round: {0} / {1}", round, max_round);
    }

    public void UpdateRoundTimeText(float round_left_time)
    {
        if (InGame_roundTimeText == null) return;
        round_left_time = round_left_time < 0f ? 0f : round_left_time;

        // total milliseconds rounded
        int totalMs = Mathf.RoundToInt(round_left_time * 1000f);
        int ms = totalMs % 1000;
        int totalSec = totalMs / 1000;
        int s = totalSec % 60;
        int m = totalSec / 60;

        InGame_roundTimeText.text = string.Format("{0:D2}:{1:D2}.{2:D3}", m, s, ms);
    }

    public void SetGameOverUI(int round, int seconds, int kill_count)
    {
        GameOverUI_roundText.text = string.Format("Round: {0}", round);
        GameOverUI_timeText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
        GameOverUI_killCountText.text = string.Format("Kill: {0}", kill_count);
    }

    public void SetGameClearUI(int round, int seconds, int kill_count)
    {
        GameClearUI_roundText.text = string.Format("Round: {0}", round);
        GameClearUI_timeText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
        GameClearUI_killCountText.text = string.Format("Kill: {0}", kill_count);
    }
}

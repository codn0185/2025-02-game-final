/* 
로드된 프로필 UI
- 저장된 프로필 정보 표시 (UID, 닉네임, 마지막 플레이 날짜 등)
- 클릭 시 해당 UID로 프로필 로드
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadedProfileUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI uidText;
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI createdAtText;
    [SerializeField] private TextMeshProUGUI lastPlayedAtText;
    [SerializeField] private TextMeshProUGUI playTimeText;

    private int uid;

    public void Setup(PlayData playData)
    {
        uid = playData.UID;

        if (uidText != null)
            uidText.text = $"UID: {playData.UID}";
        if (userNameText != null)
            userNameText.text = string.IsNullOrEmpty(playData.UserName) ? "<None>" : playData.UserName;
        if (createdAtText != null)
            createdAtText.text = $"Created At: {playData.CreatedAt.ToString("yyyy-MM-dd")}";
        if (lastPlayedAtText != null)
            lastPlayedAtText.text = $"Last Played: {FormatDateTime(playData.LastPlayedAt)}";
        if (playTimeText != null)
            playTimeText.text = $"Play Time: {FormatTimeSpan(playData.TotalPlayTime)}";
    }

    private void OnSlotClicked()
    {
        // 프로필 로드 및 활성화
        ProfileManager.Instance.LoadAndActiveProfile(uid);
        Debug.Log($"Profile {uid} loaded and activated");

        // TODO: 게임 메인 씬으로 전환
        // SceneManager.LoadScene("GameScene");
    }

    // ========== Formatting Helpers ==========
    private string FormatDateTime(DateTime dateTime)
    {
        TimeSpan timeSinceLastPlay = DateTime.Now - dateTime;

        if (timeSinceLastPlay.TotalSeconds < 5)
            return "Now";
        if (timeSinceLastPlay.TotalMinutes < 1)
            return $"{(int)timeSinceLastPlay.TotalSeconds}second(s) ago";
        if (timeSinceLastPlay.TotalHours < 1)
            return $"{(int)timeSinceLastPlay.TotalMinutes}minute(s) ago";
        if (timeSinceLastPlay.TotalDays < 1)
            return $"{(int)timeSinceLastPlay.TotalHours}hour(s) ago";
        if (timeSinceLastPlay.TotalDays <= 30)
            return $"{(int)timeSinceLastPlay.TotalDays}day(s) ago";

        return dateTime.ToString("yyyy-MM-dd");
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1)
            return $"{timeSpan.Minutes}m";
        if (timeSpan.TotalDays < 1)
            return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";

        return $"{(int)timeSpan.TotalDays}d {timeSpan.Hours}h";
    }
}

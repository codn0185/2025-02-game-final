using System;
using System.IO;

public class PlayData : UserData
{
    public string UserName { get; private set; } // 플레이어 이름
    public DateTime CreatedAt { get; private set; } = DateTime.Now; // 계정 생성 날짜
    public DateTime LastPlayedAt { get; private set; } = DateTime.Now; // 최근 플레이 날짜
    public TimeSpan TotalPlayTime { get; set; } = TimeSpan.Zero; // 총 플레이 시간

    public PlayData(int uid)
    {
        UID = uid;
    }

    public override void Save()
    {
        UpdateLastPlayed();
        DataUtil.SaveJson(this, FilePath);
    }

    public override void Delete()
    {
        DataUtil.DeleteFile(FilePath);
    }

    public void SetUserName(string userName)
    {
        UserName = userName;
        Save();
    }

    private void UpdateLastPlayed()
    {
        LastPlayedAt = DateTime.Now;
    }

    public static PlayData Load(int uid)
    {
        string filePath = Path.Combine(ProfileManager.GetProfileDir(uid), "PlayData.json");
        return DataUtil.LoadJson<PlayData>(filePath);
    }
}
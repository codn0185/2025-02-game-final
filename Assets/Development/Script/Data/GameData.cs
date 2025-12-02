using System.IO;

public class GameData : UserData
{
    public int MaxStage { get; private set; } = 0;
    public int MaxRound { get; private set; } = 0;
    public int TotalGold { get; set; } = 0; // 전체 획득 골드
    public int CurrentGold { get; set; } = 0; // 현재 보유 골드
    public int TotalGems { get; set; } = 0; // 전체 획득 보석
    public int CurrentGems { get; set; } = 0; // 현재 보유 보석
    // 업그레이드 및 해금 정보 추가 필요

    public GameData(int uid)
    {
        UID = uid;
    }

    public override void Save()
    {
        DataUtil.SaveJson(this, FilePath);
    }

    public override void Delete()
    {
        DataUtil.DeleteFile(FilePath);
    }

    public void UpdateMaxProgress(int stage, int round)
    {
        if (stage > MaxStage || (stage == MaxStage && round > MaxRound))
        {
            MaxStage = stage;
            MaxRound = round;
        }
    }

    public static GameData Load(int uid)
    {
        string filePath = Path.Combine(ProfileManager.GetProfileDir(uid), "GameData.json");
        return DataUtil.LoadJson<GameData>(filePath);
    }
}

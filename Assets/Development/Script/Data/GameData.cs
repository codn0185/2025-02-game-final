using System.IO;

public class GameData : UserData
{
    public class StageProgress
    {
        public int Stage { get; private set; }
        public int Round { get; private set; }
        public StageProgress(int stage, int round)
        {
            Stage = stage;
            Round = round;
        }

        public void Update(StageProgress stageProgress)
        {
            Stage = stageProgress.Stage;
            Round = stageProgress.Round;
        }

        public static bool operator >(StageProgress a, StageProgress b)
        {
            if (a.Stage > b.Stage) return true;
            if (a.Stage == b.Stage && a.Round > b.Round) return true;
            return false;
        }

        public static bool operator <(StageProgress a, StageProgress b)
        {
            if (a.Stage < b.Stage) return true;
            if (a.Stage == b.Stage && a.Round < b.Round) return true;
            return false;
        }
    }

    public StageProgress MaxStageProgress { get; private set; } = new StageProgress(0, 0);
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

    public void UpdateMaxStageProgress(StageProgress newProgress)
    {
        if (newProgress > MaxStageProgress)
        {
            MaxStageProgress.Update(newProgress);
        }
    }

    public void UpdateMaxStageProgress(int stage, int round)
    {
        UpdateMaxStageProgress(new StageProgress(stage, round));
    }

    public static GameData Load(int uid)
    {
        string filePath = Path.Combine(ProfileManager.GetProfileDir(uid), "GameData.json");
        return DataUtil.LoadJson<GameData>(filePath);
    }
}

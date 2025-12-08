public class StageSettings
{
    public static int MaxStageNumber => 3;
    public const string Stage1 = "StageSettings1"; // 게임 실행 시 처음 로드되는 씬
    public const string Stage2 = "StageSettings2"; // 게임 로비 씬
    public const string Stage3 = "StageSettings3"; // 게임 플레이 씬

    public static string GetStageFromNumber(int stageNumber)
    {
        return stageNumber switch
        {
            1 => Stage1,
            2 => Stage2,
            3 => Stage3,
            _ => null,
        };
    }

}
public class UserData : Data
{
    public int UID { get; private set; }
    public string PlayerName { get; set; }

    // 계정 생성 날짜
    public System.DateTime CreatedAt { get; private set; }
    // 최근 플레이 날짜
    public System.DateTime LastPlayedAt { get; set; }
    public string FilePath => $"PlayerData/{UID}.json";

    public UserData()
    {
        // 중복되지 않는 UID 생성 (파일 경로 기준)
        do
        {
            UID = UnityEngine.Random.Range(100000, 999999);
        } while (System.IO.File.Exists(System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, $"PlayerData/{UID}.json")));

        CreatedAt = System.DateTime.Now;
        LastPlayedAt = System.DateTime.Now;
    }

    public void UpdateLastPlayed()
    {
        LastPlayedAt = System.DateTime.Now;
    }
}
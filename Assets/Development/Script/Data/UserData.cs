using System.IO;

public abstract class UserData : Data
{
    // 고유 사용자 ID
    public int UID { get; protected set; }
    // 파일명 (클래스 이름 사용)
    protected virtual string FileName => GetType().Name;
    // 파일 경로
    protected string FilePath => Path.Combine(ProfileManager.GetProfileDir(UID), $"{FileName}.json");
}
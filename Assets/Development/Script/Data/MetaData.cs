using System.Collections.Generic;
using System.IO;

public class MetaData : UserData
{
    private readonly Dictionary<MetaUpgradeType, int> upgradeLevels = new();
    public MetaData(int uid)
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

    public void SetUpgradeLevel(MetaUpgradeType type, int level)
    {
        upgradeLevels[type] = level;
    }

    public int GetUpgradeLevel(MetaUpgradeType type)
    {
        if (!upgradeLevels.ContainsKey(type))
        {
            upgradeLevels[type] = 0;
        }
        return upgradeLevels[type];
    }

    public static MetaData Load(int uid)
    {
        string filePath = Path.Combine(ProfileManager.GetProfileDir(uid), "MetaData.json");
        return DataUtil.LoadJson<MetaData>(filePath);
    }
}
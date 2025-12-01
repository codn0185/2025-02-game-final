using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserData : Data
{
    private static readonly WaitForSeconds AutoSaveInterval = new(60f);

    private static int UIDLength = 8;
    public static string Dir => Path.Combine(DataUtil.Dir, "PlayerData");
    public static string FilePath(int uid) => $"{Dir}/{uid}.json";

    public int UID { get; private set; } // 고유 사용자 ID
    public string UserName { get; private set; } // 플레이어 이름
    public System.DateTime CreatedAt { get; private set; } // 계정 생성 날짜
    public System.DateTime LastPlayedAt { get; private set; } // 최근 플레이 날짜

    private readonly MonoBehaviour monoBehaviour;
    private Coroutine autoSaveCoroutine;

    public UserData(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
        do
        {
            UID = Random.Range((int)Mathf.Pow(10, UIDLength - 1), (int)Mathf.Pow(10, UIDLength) - 1);
        } while (File.Exists(Path.Combine(Application.persistentDataPath, FilePath(UID))));

        CreatedAt = System.DateTime.Now;
    }

    public UserData(MonoBehaviour monoBehaviour, string userName) : this(monoBehaviour)
    {
        SetName(userName);
    }

    public void Enter()
    {
        StartAutoSaveCoroutine();
    }

    public void Exit()
    {
        StopAutoSaveCoroutine();
        UpdateLastPlayed();
        Save();
    }

    public void Save()
    {
        DataUtil.SaveJson(this, FilePath(UID));
    }

    public void Delete()
    {
        DataUtil.DeleteFile(FilePath(UID));
    }

    public void SetName(string playerName)
    {
        UserName = playerName;
        Save();
    }

    private void UpdateLastPlayed()
    {
        LastPlayedAt = System.DateTime.Now;
    }

    private void StartAutoSaveCoroutine()
    {
        if (autoSaveCoroutine == null)
            autoSaveCoroutine = monoBehaviour.StartCoroutine(AutoSaveCoroutine());

    }

    private void StopAutoSaveCoroutine()
    {
        if (autoSaveCoroutine != null)
        {
            monoBehaviour.StopCoroutine(autoSaveCoroutine);
            autoSaveCoroutine = null;
        }
    }

    public IEnumerator AutoSaveCoroutine()
    {
        yield return AutoSaveInterval;
        UpdateLastPlayed();
        Save();
    }

    public static UserData Load(int uid)
    {
        return DataUtil.LoadJson<UserData>(FilePath(uid));
    }

    public static List<UserData> LoadAll()
    {
        List<UserData> allUserData = new();

        if (Directory.Exists(Dir))
        {
            string[] files = Directory.GetFiles(Dir, "*.json");
            foreach (string file in files)
            {
                UserData userData = Load(int.Parse(Path.GetFileNameWithoutExtension(file)));
                if (userData == null)
                {
                    File.Delete(file);
                    continue;
                }
                allUserData.Add(userData);
            }
        }

        return allUserData;
    }
}
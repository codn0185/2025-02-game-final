using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ProfileState
{
    Inactive,   // 비활성 상태 (프로필 선택 전 또는 종료 후)
    Active      // 활성 상태 (프로필 로드 완료 및 게임 진행 중)
}

public class ProfileManager : Singleton<ProfileManager>
{
    // ========== Static Constants ==========
    private static readonly int UIDLength = 8;
    private static readonly WaitForSeconds AutoSaveInterval = new(60f);

    // ========== Static Properties ==========
    public static string ProfileRootDir => Path.Combine(DataUtil.Dir, "Profiles");
    public static string GetProfileDir(int uid) => Path.Combine(ProfileRootDir, uid.ToString());

    // ========== Data Fields ==========
    public int UID { get; private set; } = -1;
    public PlayData PlayData { get; private set; } = null;
    public GameData GameData { get; private set; } = null;

    // ========== State ==========
    public ProfileState CurrentState { get; private set; } = ProfileState.Inactive;

    // ========== etc ==========
    private Coroutine autoSaveCoroutine;


    // ========== Unity Lifecycle ==========
    protected override void Awake()
    {
        base.Awake();
        // 프로필 루트 디렉토리 생성
        if (!Directory.Exists(ProfileRootDir))
        {
            DataUtil.CreateDirectory(ProfileRootDir);
        }
    }

    // ========== Profile Management - Public Methods ==========
    public void CreateProfile()
    {
        UID = GenerateRandomUID();
        DataUtil.CreateDirectory(GetProfileDir(UID));
        PlayData = new PlayData(UID);
        GameData = new GameData(UID);
        SaveActiveProfile();
    }

    public void CreateAndActiveProfile()
    {
        CreateProfile();
        ActivateProfile();
    }

    public void LoadProfile(int uid)
    {
        if (!Directory.Exists(GetProfileDir(uid)))
            DataUtil.CreateDirectory(GetProfileDir(uid));
        UID = uid;
        PlayData = PlayData.Load(uid) ?? new PlayData(uid);
        GameData = GameData.Load(uid) ?? new GameData(uid);
        SaveActiveProfile();
    }

    public void LoadAndActiveProfile(int uid)
    {
        LoadProfile(uid);
        ActivateProfile();
    }

    public void SaveActiveProfile()
    {
        PlayData?.Save();
        GameData?.Save();
    }

    public void DeleteProfile(int uid)
    {
        string profileDir = GetProfileDir(uid);
        if (Directory.Exists(profileDir))
        {
            DataUtil.DeleteDirectory(profileDir);
        }
    }

    public static List<int> GetAllProfileUIDs()
    {
        List<int> uids = new();
        string[] profileDirs = Directory.GetDirectories(ProfileRootDir);
        foreach (string dir in profileDirs)
        {
            string dirName = Path.GetFileName(dir);
            if (int.TryParse(dirName, out int uid))
            {
                uids.Add(uid);
            }
        }
        return uids;
    }

    // ========== Profile Lifecycle ==========
    public void EnterProfile()
    {
        ActivateProfile();
    }

    public void ExitProfile()
    {
        SaveActiveProfile();
        DeactivateProfile();
    }

    public void ActivateProfile()
    {
        if (CurrentState == ProfileState.Inactive)
        {
            ChangeState(ProfileState.Active);
        }
    }

    public void DeactivateProfile()
    {
        if (CurrentState == ProfileState.Active)
        {
            ChangeState(ProfileState.Inactive);
        }
    }

    private void ChangeState(ProfileState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        switch (CurrentState)
        {
            case ProfileState.Active:
                StartAutoSave();
                break;
            case ProfileState.Inactive:
                StopAutoSave();
                SaveActiveProfile();
                ClearActiveProfile();
                break;
        }
    }

    // ========== User Data Helpers ==========
    public void SetUserName(string userName)
    {
        PlayData?.SetUserName(userName);
    }

    // ========== Auto Save ==========
    public void StartAutoSave()
    {
        if (autoSaveCoroutine == null)
            autoSaveCoroutine = StartCoroutine(AutoSaveCoroutine());
    }

    public void StopAutoSave()
    {
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
            autoSaveCoroutine = null;
        }
    }

    private IEnumerator AutoSaveCoroutine()
    {
        while (CurrentState == ProfileState.Active)
        {
            yield return AutoSaveInterval;
            SaveActiveProfile();
        }
    }

    // ========== Private Helper Methods ==========

    private int GenerateRandomUID()
    {
        int uid;
        do
        {
            uid = Random.Range((int)Mathf.Pow(10, UIDLength - 1), (int)Mathf.Pow(10, UIDLength) - 1);
        } while (Directory.Exists(GetProfileDir(uid)));
        return uid;
    }

    private void ClearActiveProfile()
    {
        UID = -1;
        PlayData = null;
        GameData = null;
    }
}
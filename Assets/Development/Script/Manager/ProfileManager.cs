/* 
프로필 관리

- 현재 저장된 모든 프로필 미리보기
    foreach (int uid in ProfileManager.GetAllProfileUIDs()) { 
        PlayData playData = PlayData.Load(uid);
        GameData gameData = GameData.Load(uid);
        // 프로필 정보 사용
    }

- 새 프로필 생성
    ProfileManager.Instance.CreateNewProfile();

- 기존 프로필 불러오기 및 활성화
    ProfileManager.Instance.LoadProfile(existingUID);

- 활성 프로필 저장
    ProfileManager.Instance.SaveActiveProfile();

- 활성 프로필 삭제
    ProfileManager.Instance.DeleteActiveProfile();

- 활성 프로필 진입 및 퇴장
    ProfileManager.Instance.EnterProfile();
    ProfileManager.Instance.ExitProfile();

- 사용자 이름 설정
    ProfileManager.Instance.SetUserName("NewPlayer");
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ProfileState
{
    Inactive,   // 비활성 상태 (프로필 선택 전 또는 종료 후)
    Active      // 활성 상태 (프로필 로드 완료 및 게임 진행 중)
}

public class ProfileManager : MonoBehaviour
{
    // ========== Singleton ==========
    public static ProfileManager Instance { get; private set; }

    // ========== Static Constants ==========
    private static readonly int UIDLength = 8;
    private static readonly WaitForSeconds AutoSaveInterval = new(60f);

    // ========== Static Properties ==========
    public static string ProfileRootDir => Path.Combine(DataUtil.Dir, "Profiles");
    public static string GetProfileDir(int uid) => Path.Combine(ProfileRootDir, uid.ToString());

    // ========== Instance Fields ==========
    public int UID { get; private set; } = -1;
    public PlayData PlayData { get; private set; } = null;
    public GameData GameData { get; private set; } = null;
    private Coroutine autoSaveCoroutine;
    private ProfileState currentState = ProfileState.Inactive;

    // ========== Properties ==========
    public ProfileState CurrentState => currentState;

    // ========== Unity Lifecycle ==========
    public ProfileManager()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DataUtil.CreateDirectory(ProfileRootDir);
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
        if (currentState == ProfileState.Inactive)
        {
            ChangeState(ProfileState.Active);
        }
    }

    public void DeactivateProfile()
    {
        if (currentState == ProfileState.Active)
        {
            ChangeState(ProfileState.Inactive);
        }
    }

    private void ChangeState(ProfileState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        switch (currentState)
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
        while (currentState == ProfileState.Active)
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
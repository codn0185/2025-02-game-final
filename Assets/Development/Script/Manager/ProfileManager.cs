using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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
    public static readonly int MAX_PROFILE_COUNT = 3;

    // ========== Static Properties ==========
    public static string ProfileRootDir => Path.Combine(DataUtil.Dir, "Profiles");
    public static string GetProfileDir(int uid) => Path.Combine(ProfileRootDir, uid.ToString());

    // ========== UI Prefab References ==========
    [Header("Profile Slot Container")]
    [SerializeField] private Transform profileSlotContainer;

    [Header("Profile UI Prefabs")]
    [SerializeField] private GameObject loadedProfileUIPrefab;
    [SerializeField] private GameObject emptyProfileUIPrefab;

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

    public static bool CanCreateNewProfile()
    {
        return GetAllProfileUIDs().Count < MAX_PROFILE_COUNT;
    }

    public bool TryCreateAndActiveProfile()
    {
        if (!CanCreateNewProfile()) return false;

        CreateAndActiveProfile();
        return true;
    }

    // ========== Profile UI Management ==========

    // 프로필 슬롯 UI 초기 설정
    public void InitializeProfileSlotUI()
    {
        // Horizontal Layout Group 확인 및 추가
        GridLayoutGroup layoutGroup = profileSlotContainer.GetComponent<GridLayoutGroup>();

        // 레이아웃 설정
        if (layoutGroup == null)
        {
            layoutGroup = profileSlotContainer.gameObject.AddComponent<GridLayoutGroup>();
            layoutGroup.cellSize = new Vector2(200, 100);
            layoutGroup.spacing = new Vector2(10, 10);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            layoutGroup.constraintCount = 1;
        }

        RefreshProfileUI();
    }

    // 프로필 슬롯 UI 초기화
    public void RefreshProfileUI()
    {
        // 기존 슬롯 모두 제거
        ClearProfileSlots();

        // 저장된 프로필 UID 가져오기
        List<int> LoadedUIDs = GetAllProfileUIDs();

        // 생성일 기준 오름차순 정렬 (가장 오래된 프로필부터)
        LoadedUIDs.Sort((uid1, uid2) =>
        {
            PlayData playData1 = PlayData.Load(uid1);
            PlayData playData2 = PlayData.Load(uid2);

            if (playData1 == null && playData2 == null) return 0;
            if (playData1 == null) return 1;
            if (playData2 == null) return -1;

            return playData1.CreatedAt.CompareTo(playData2.CreatedAt);
        });

        // 정렬된 순서로 슬롯 추가
        foreach (int uid in LoadedUIDs)
        {
            AddLoadedProfileSlot(uid);
        }

        // 남은 빈 슬롯 채우기
        int remainingSlots = MAX_PROFILE_COUNT - LoadedUIDs.Count;
        for (int i = 0; i < remainingSlots; i++)
        {
            AddEmptyProfileSlot();
        }
    }

    // 모든 프로필 슬롯 제거
    public void ClearProfileSlots()
    {
        int childCount = profileSlotContainer.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(profileSlotContainer.GetChild(i).gameObject);
        }
    }

    // 로드된 프로필 슬롯 추가
    public void AddLoadedProfileSlot(int uid)
    {
        GameObject slotObj = Instantiate(loadedProfileUIPrefab, profileSlotContainer);
        SetupSlotTransform(slotObj);

        LoadedProfileUI slot = slotObj.GetComponent<LoadedProfileUI>();
        if (slot != null)
        {
            PlayData playData = PlayData.Load(uid);
            if (playData != null)
            {
                Debug.Log($"ProfileManager: Created profile slot for UID {uid}");
                slot.Setup(playData);
            }
            else
            {
                Debug.LogWarning($"ProfileManager: Failed to load PlayData for UID {uid}");
                Destroy(slotObj);
                AddEmptyProfileSlot();
            }
        }
        else
        {
            Debug.LogWarning("ProfileManager: LoadedProfileUI component not found on the instantiated prefab.");
            Destroy(slotObj);
            AddEmptyProfileSlot();
        }
    }

    // 빈 프로필 슬롯 추가
    public void AddEmptyProfileSlot()
    {
        Debug.Log("ProfileManager: Created empty profile slot");
        GameObject slotObj = Instantiate(emptyProfileUIPrefab, profileSlotContainer);
        SetupSlotTransform(slotObj);
    }

    // ========== Private Helper Methods ==========

    private void SetupSlotTransform(GameObject slotObj)
    {
        RectTransform rectTransform = slotObj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
        }
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
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 업그레이드 UI 패널 관리
/// MetaManager의 모든 업그레이드 SO를 가져와서 슬롯 생성
/// </summary>
public class UpgradePanel : MonoBehaviour
{
    [Header("Prefab & Container")]
    [SerializeField] private GameObject upgradeSlotPrefab; // UpgradeSlot 프리팹
    [SerializeField] private Transform slotContainer; // 슬롯이 생성될 부모 Transform

    [Header("Grid Layout Settings")]
    [SerializeField] private int columns = 4; // 가로 개수
    [SerializeField] private int rows = 2; // 세로 개수
    [SerializeField] private Vector2 spacing = new(20, 20); // 슬롯 간격 (가로, 세로)
    [SerializeField] private Vector2 cellSize = new(300, 400); // 각 슬롯 크기

    private void Start()
    {
        SetupGridLayout();
        InitializeUpgradeSlots();
    }

    /// <summary>
    /// Grid Layout Group 설정
    /// </summary>
    private void SetupGridLayout()
    {
        if (slotContainer == null) return;

        // Grid Layout Group 컴포넌트 가져오기 또는 추가
        GridLayoutGroup gridLayout = slotContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = slotContainer.gameObject.AddComponent<GridLayoutGroup>();
        }

        // 격자 설정
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        // 셀 크기 설정 (프리팹 크기 무시하고 강제 적용)
        gridLayout.cellSize = cellSize;

        // 간격 및 정렬 설정
        gridLayout.spacing = spacing; // 인스펙터에서 설정한 간격 사용
        gridLayout.childAlignment = TextAnchor.UpperCenter; // 정렬

        Debug.Log($"UpgradePanel: Grid Layout set to {columns} columns x {rows} rows");
    }

    /// <summary>
    /// 모든 업그레이드 슬롯 생성 및 초기화
    /// </summary>
    private void InitializeUpgradeSlots()
    {
        if (upgradeSlotPrefab == null)
        {
            Debug.LogError("UpgradePanel: upgradeSlotPrefab is null!");
            return;
        }

        if (slotContainer == null)
        {
            Debug.LogError("UpgradePanel: slotContainer is null!");
            return;
        }

        // MetaManager에서 모든 업그레이드 SO 가져오기
        MetaUpgradeSO[] upgrades = MetaManager.Instance.MetaUpgrades;

        if (upgrades == null || upgrades.Length == 0)
        {
            Debug.LogWarning("UpgradePanel: No upgrades found in MetaManager!");
            return;
        }

        // 각 업그레이드마다 슬롯 생성
        foreach (var upgradeSO in upgrades)
        {
            if (upgradeSO == null) continue;

            // 슬롯 프리팹 생성
            GameObject slotObj = Instantiate(upgradeSlotPrefab, slotContainer);

            // UpgradeSlot 컴포넌트 가져오기
            UpgradeSlot slot = slotObj.GetComponent<UpgradeSlot>();

            if (slot != null)
            {
                // SO 데이터로 슬롯 초기화 (자동으로 UI 설정됨)
                slot.Initialize(upgradeSO);
            }
            else
            {
                Debug.LogError("UpgradePanel: UpgradeSlot component not found on prefab!");
            }
        }

        Debug.Log($"UpgradePanel: {upgrades.Length} upgrade slots created.");
    }

    /// <summary>
    /// 모든 슬롯 UI 새로고침 (재화 변경 시 등)
    /// </summary>
    public void RefreshAllSlots()
    {
        UpgradeSlot[] slots = slotContainer.GetComponentsInChildren<UpgradeSlot>();
        foreach (var slot in slots)
        {
            slot.Refresh();
        }
    }
}

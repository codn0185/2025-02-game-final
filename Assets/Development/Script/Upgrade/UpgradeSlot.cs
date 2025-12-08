using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 메타 업그레이드 UI 슬롯 프리팹에 부착
/// SO 데이터를 받아서 UI를 자동으로 설정하고 업그레이드 처리
/// </summary>
public class UpgradeSlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image imageIcon;
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text costText;
    [SerializeField] private Button upgradeButton;

    private MetaUpgradeSO upgradeSO;
    private MetaUpgradeType upgradeType;

    /// <summary>
    /// SO 데이터로 슬롯 초기화 및 UI 설정
    /// </summary>
    public void Initialize(MetaUpgradeSO so)
    {
        if (so == null)
        {
            Debug.LogError("UpgradeSlot: MetaUpgradeSO is null!");
            return;
        }

        upgradeSO = so;
        upgradeType = so.UpgradeType;
        
        // 기본 정보 설정 (변하지 않는 정보)
        SetImage(so.Icon);
        SetTitle(so.Name);
        SetDescription(so.Description);
        
        // 업그레이드 버튼 리스너 설정
        SetUpgradeButtonListener(OnUpgradeButtonClicked);
        
        // 동적 정보 업데이트 (레벨, 비용, 버튼 상태)
        UpdateUI();
    }

    /// <summary>
    /// 아이콘 설정
    /// </summary>
    public void SetImage(Sprite icon)
    {
        if (imageIcon != null && icon != null)
            imageIcon.sprite = icon;
    }

    /// <summary>
    /// 제목 설정
    /// </summary>
    public void SetTitle(string title)
    {
        if (titleText != null)
            titleText.text = title;
    }

    /// <summary>
    /// 설명 설정
    /// </summary>
    public void SetDescription(string description)
    {
        if (descriptionText != null)
            descriptionText.text = description;
    }

    /// <summary>
    /// 레벨 표시
    /// </summary>
    public void SetLevel(int currentLevel, int maxLevel)
    {
        if (levelText != null)
            levelText.text = $"{currentLevel} / {maxLevel}";
    }

    /// <summary>
    /// 비용 표시
    /// </summary>
    public void SetCost(int cost)
    {
        if (costText != null)
            costText.text = $"{cost} Gold";
    }

    /// <summary>
    /// 최대 레벨 표시
    /// </summary>
    public void SetMaxLevel()
    {
        if (costText != null)
            costText.text = "MAX";
    }

    /// <summary>
    /// 버튼 활성화/비활성화
    /// </summary>
    public void SetButtonInteractable(bool interactable)
    {
        if (upgradeButton != null)
            upgradeButton.interactable = interactable;
    }

    /// <summary>
    /// 업그레이드 버튼 리스너 설정
    /// </summary>
    public void SetUpgradeButtonListener(UnityAction action)
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(action);
        }
    }

    /// <summary>
    /// UI 업데이트 (레벨, 비용, 버튼 상태)
    /// </summary>
    private void UpdateUI()
    {
        if (upgradeSO == null) return;

        // 현재 레벨 가져오기
        int currentLevel = MetaManager.Instance.GetUpgradeLevel(upgradeType);
        
        // 레벨 표시
        SetLevel(currentLevel, upgradeSO.MaxLevel);
        
        // 비용 및 버튼 상태
        if (currentLevel >= upgradeSO.MaxLevel)
        {
            // 최대 레벨 - 버튼 비활성화
            SetMaxLevel();
            SetButtonInteractable(false);
        }
        else
        {
            // 업그레이드 가능 - 재화 부족해도 버튼은 활성화
            int cost = upgradeSO.GetCost(currentLevel);
            SetCost(cost);
            SetButtonInteractable(true);
        }
    }

    /// <summary>
    /// 업그레이드 버튼 클릭 시 호출
    /// </summary>
    private void OnUpgradeButtonClicked()
    {
        if (upgradeSO == null) return;

        // MetaManager를 통해 업그레이드 시도
        bool success = MetaManager.Instance.TryUpgrade(upgradeType);
        
        if (success)
        {
            Debug.Log($"{upgradeSO.Name} 업그레이드 성공!");
            
            // UI 갱신
            UpdateUI();
            
            // 재화 UI 업데이트 (다른 슬롯들도 함께 갱신됨)
            MetaManager.Instance.UpdateResourcesUI();
        }
        else
        {
            // 실패 원인 확인
            int currentLevel = MetaManager.Instance.GetUpgradeLevel(upgradeType);
            if (currentLevel >= upgradeSO.MaxLevel)
            {
                Debug.Log($"{upgradeSO.Name} 이미 최대 레벨입니다.");
            }
            else
            {
                int cost = upgradeSO.GetCost(currentLevel);
                int currentGold = MetaManager.Instance.CurrentGold;
                Debug.Log($"{upgradeSO.Name} 재화 부족! (필요: {cost}, 보유: {currentGold})");
            }
        }
    }

    /// <summary>
    /// 모든 슬롯 UI 업데이트 (재화 변경 시)
    /// </summary>
    private void UpdateAllSlots()
    {
        UpgradeSlot[] allSlots = FindObjectsByType<UpgradeSlot>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
        {
            if (slot != this) // 자기 자신은 이미 업데이트됨
            {
                slot.UpdateUI();
            }
        }
    }

    /// <summary>
    /// 외부에서 UI 갱신 (재화 변경 시 등)
    /// </summary>
    public void Refresh()
    {
        UpdateUI();
    }
}
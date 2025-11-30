using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 진행 중 획득한 아이템을 관리하는 매니저 클래스
/// 재화, 소모, 강화 아이템을 각각 추적하고 지속형 소모 아이템의 효과를 관리합니다.
/// </summary>
public class GameItemManager : MonoBehaviour
{
    public static GameItemManager Instance { get; private set; }

    // 아이템 획득 데이터 저장

    private readonly Dictionary<ResourceType, int> _resources = new();
    private readonly Dictionary<UpgradeType, int> _upgrades = new();
    private readonly Dictionary<InstantConsumableType, int> _instantConsumables = new();
    private readonly Dictionary<TimedConsumableType, int> _timedConsumables = new();

    public Dictionary<ResourceType, int> Resources => _resources;
    public Dictionary<UpgradeType, int> Upgrades => _upgrades;
    public Dictionary<InstantConsumableType, int> InstantConsumables => _instantConsumables;
    public Dictionary<TimedConsumableType, int> TimedConsumables => _timedConsumables;

    // TimedEffectItem 관리
    private class TimedEffect
    {
        public TimedConsumableItem item;
        public float remainingTime;
        public TimedEffect(TimedConsumableItem item, float remainingTime)
        {
            this.item = item;
            this.remainingTime = remainingTime;
        }
    }

    private readonly List<TimedEffect> _activeEffects = new();

    // === 생명주기 메서드 ===

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // 지속형 아이템 효과 관리
    }

    // === 아이템 추가 메서드 ===

    public void AddResource(ResourceItem item)
    {
        if (_resources.ContainsKey(item.type))
            _resources[item.type] += item.amount;
        else
            _resources[item.type] = item.amount;
    }

    public void AddUpgrade(UpgradeItem item)
    {
        if (item == null) return;
        if (_upgrades.ContainsKey(item.type))
            _upgrades[item.type]++;
        else
            _upgrades[item.type] = 1;
        item.Apply();
    }

    public void AddInstantConsumable(InstantConsumableItem item)
    {
        if (item == null) return;
        if (_instantConsumables.ContainsKey(item.type))
            _instantConsumables[item.type]++;
        else
            _instantConsumables[item.type] = 1;
        item.Apply();
    }

    public void AddTimedConsumable(TimedConsumableItem item)
    {
        if (item == null) return;
        if (_timedConsumables.ContainsKey(item.type))
            _timedConsumables[item.type]++;
        else
            _timedConsumables[item.type] = 1;
        item.Apply();
        _activeEffects.Add(new TimedEffect(item, item.duration));
    }

    // === 지속형 아이템 효과 관리 메서드 ===

    public void UpdateTimedEffects(float deltaTime)
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            TimedEffect effect = _activeEffects[i];
            effect.remainingTime -= deltaTime;

            if (effect.remainingTime <= 0)
            {
                effect.item.Revert();
                _activeEffects.RemoveAt(i);
            }
        }
    }

    // === 기타 메서드 ===

    // 아이템 획득 정보 초기화
    public void Clear()
    {
        _resources.Clear();
        _upgrades.Clear();
        _instantConsumables.Clear();
        _timedConsumables.Clear();
        // 모든 활성 효과 Revert() 실행
        foreach (var effect in _activeEffects)
        {
            effect.item.Revert();
        }
        _activeEffects.Clear();
    }
}


using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 진행 중 획득한 아이템을 관리하는 매니저 클래스
/// 재화, 소모, 강화 아이템을 각각 추적하고 지속형 소모 아이템의 효과를 관리합니다.
/// </summary>
public class RunItemManager : Singleton<RunItemManager>
{
    // 아이템 획득 데이터 저장
    public Dictionary<ResourceType, int> Resources { get; private set; } = new();
    public Dictionary<UpgradeType, int> Upgrades { get; private set; } = new();
    public Dictionary<InstantConsumableType, int> InstantConsumables { get; private set; } = new();
    public Dictionary<TimedConsumableType, int> TimedConsumables { get; private set; } = new();

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

    // === Unity Lifecycle ===
    void Update()
    {
        UpdateTimedEffects();
    }


    // === 아이템 추가 메서드 ===
    public void AddResource(ResourceItem item)
    {
        if (item == null) return;
        if (Resources.ContainsKey(item.type))
            Resources[item.type] += item.amount;
        else
            Resources[item.type] = item.amount;
    }

    public void AddUpgrade(UpgradeItem item)
    {
        if (item == null) return;
        if (Upgrades.ContainsKey(item.type))
            Upgrades[item.type]++;
        else
            Upgrades[item.type] = 1;
        item.Apply();
    }

    public void AddInstantConsumable(InstantConsumableItem item)
    {
        if (item == null) return;
        if (InstantConsumables.ContainsKey(item.type))
            InstantConsumables[item.type]++;
        else
            InstantConsumables[item.type] = 1;
        item.Apply();
    }

    public void AddTimedConsumable(TimedConsumableItem item)
    {
        if (item == null) return;
        if (TimedConsumables.ContainsKey(item.type))
            TimedConsumables[item.type]++;
        else
            TimedConsumables[item.type] = 1;
        item.Apply();
        _activeEffects.Add(new TimedEffect(item, item.duration));
    }

    // === 지속형 아이템 효과 관리 메서드 ===
    public void UpdateTimedEffects()
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            TimedEffect effect = _activeEffects[i];
            effect.remainingTime -= Time.deltaTime;

            if (effect.remainingTime <= 0)
            {
                effect.item.Revert();
                _activeEffects.RemoveAt(i);
                i--;
            }
        }
    }

    // === 기타 메서드 ===

    // 아이템 획득 정보 초기화
    public void Clear()
    {
        Resources.Clear();
        Upgrades.Clear();
        InstantConsumables.Clear();
        TimedConsumables.Clear();
        // 모든 활성 효과 Revert() 실행
        foreach (var effect in _activeEffects)
        {
            effect.item.Revert();
        }
        _activeEffects.Clear();
    }
}


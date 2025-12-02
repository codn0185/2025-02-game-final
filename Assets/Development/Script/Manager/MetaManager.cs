/* 
프로필에 반영되는 강화 및 해금 등을 관리하는 싱글톤 매니저 클래스
    - 강화: 마법 종류에 따라 기본 능력치가 다르므로 증가 비율로 관리
    - 스킬 해금: 해금된 스킬 목록 관리
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public enum MetaUpgradeType
{
    BaseHealth,
    BaseAttackPower,
    BaseAttackSpeed,
    BaseHeal,
}

public class MetaManager : Singleton<MetaManager>
{
    public MetaUpgradeSO[] MetaUpgrades; // 인스펙터에서 설정

    private readonly Dictionary<MetaUpgradeType, int> MetaUpgradeLevels = new();
    private readonly Dictionary<MetaUpgradeType, MetaUpgradeSO> upgradeSODict = new();

    public int CurrentGold { get; private set; } = 0;
    public int CurrentGem {get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    public void Initialize()
    {
        // SO Dictionary 구축 (O(1) 접근)
        upgradeSODict.Clear();
        foreach (var so in MetaUpgrades)
        {
            if (so != null)
                upgradeSODict[so.Type] = so;
        }

        foreach (var so in MetaUpgrades)
        {
            MetaUpgradeLevels[so.Type] = 0;
        }
    }

    public void ApplyFromData(MetaData data)
    {
        if (data == null) return;
        foreach (var so in MetaUpgrades)
        {
            MetaUpgradeLevels[so.Type] = data.GetUpgradeLevel(so.Type);
        }
    }

    public void SetResources(int gold, int gem)
    {
        CurrentGold = gold;
        CurrentGem = gem;
    }

    public void AddResources(int gold, int gem)
    {
        CurrentGold += gold;
        CurrentGem += gem;
    }

    public bool TryUpgrade(MetaUpgradeType type)
    {
        // Dictionary에서 O(1)로 찾기
        if (!upgradeSODict.TryGetValue(type, out MetaUpgradeSO upgradeSO))
            return false;

        // 최대 레벨 확인
        int currentLevel = MetaUpgradeLevels[type];
        if (currentLevel >= upgradeSO.MaxLevel) return false;
        // 필요한 재화 확인
        if (CurrentGold < upgradeSO.GetCost(currentLevel)) return false;

        CurrentGold -= upgradeSO.GetCost(currentLevel);
        MetaUpgradeLevels[type] = currentLevel + 1;
        return true;
    }

    // SO 정보 직접 가져오기 (UI 표시용)
    public MetaUpgradeSO GetUpgradeSO(MetaUpgradeType type)
    {
        upgradeSODict.TryGetValue(type, out MetaUpgradeSO so);
        return so;
    }

    // 현재 레벨 가져오기
    public int GetUpgradeLevel(MetaUpgradeType type)
    {
        return MetaUpgradeLevels.TryGetValue(type, out int level) ? level : 0;
    }
}
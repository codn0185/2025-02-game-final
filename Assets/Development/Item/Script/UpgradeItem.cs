// 업그레이드 아이템
using UnityEngine;

public enum UpgradeType
{
    DamageUpgrade, // 공격력 증가
    AttackSpeedUpgrade, // 공격 속도 증가
    ShieldUpgrade, // 보호막 강화
}

public abstract class UpgradeItem : ItemBase
{
    public UpgradeType type;

    public override void Apply()
    {
        RunItemManager.Instance.AddUpgrade(this);
    }
}

public class DamageUpgradeItem : UpgradeItem
{
    private void Awake()
    {
        type = UpgradeType.DamageUpgrade;
    }
}

public class AttackSpeedUpgradeItem : UpgradeItem
{
    private void Awake()
    {
        type = UpgradeType.AttackSpeedUpgrade;
    }
}

public class ShieldUpgradeItem : UpgradeItem
{
    private void Awake()
    {
        type = UpgradeType.ShieldUpgrade;
    }

}
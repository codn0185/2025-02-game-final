// 업그레이드 아이템
enum UpgradeType
{
    DamageUpgrade, // 공격력 증가
    AttackSpeedUpgrade, // 공격 속도 증가
    ShieldUpgrade, // 보호막 강화
}
public abstract class UpgradeItem : ItemBase
{
    UpgradeType type;

    public override void Apply()
    {
        // 업그레이드 아이템 효과 적용 로직 구현
        // switch (type)
        // {
        //     case UpgradeType.DamageUpgrade:
        //         PlayerController.IncreaseDamage(upgradeAmount);
        //         break;
        //     case UpgradeType.AttackSpeedUpgrade:
        //         PlayerController.IncreaseAttackSpeed(upgradeAmount);
        //         break;
        //     case UpgradeType.ShieldUpgrade:
        //         PlayerController.StrengthenShield(upgradeAmount);
        //         break;
        // }
    }
}
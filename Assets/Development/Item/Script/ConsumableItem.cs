// 소비 아이템 (회복, 일시 강화 등)
enum ConsumableType
{
    HealthPotion,
    StrengthBoost,
    AttackSpeedBoost
}
public abstract class ConsumableItem : ItemBase
{
    ConsumableType type;

    public override void Apply()
    {
        // 소비 아이템 효과 적용 로직 구현
        // switch (type)
        // {
        //     case ConsumableType.HealthPotion:
        //         PlayerController.Heal(healAmount);
        //         break;
        //     case ConsumableType.StrengthBoost:
        //         PlayerController.ApplyStrengthBoost(boostAmount, duration);
        //         break;
        //     case ConsumableType.AttackSpeedBoost:
        //         PlayerController.ApplyAttackSpeedBoost(boostAmount, duration);
        //         break;
        // }
    }
}
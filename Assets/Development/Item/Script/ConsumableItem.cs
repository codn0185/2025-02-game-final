// 소비 아이템 (회복, 일시 강화 등)
public enum ConsumableType
{
    HealthPotion,
    StrengthBoost,
    AttackSpeedBoost
}

public abstract class ConsumableItem : ItemBase
{
    public ConsumableType type;
    public bool isInstant; // true면 즉시 적용, false면 duration 동안 적용
    public float duration; // 지속형 효과일 때 지속 시간(초)

    public override void Apply()
    {
        // 소비 아이템 효과 적용 로직 구현
        // PlayerInventory.UseConsumable(type, isInstant, duration);
    }
}
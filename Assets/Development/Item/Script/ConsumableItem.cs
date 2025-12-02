// 소비 아이템 (회복, 일시 강화 등)
public abstract class ConsumableItem : ItemBase
{
}

public enum InstantConsumableType
{
    HealthPotion,
}

public abstract class InstantConsumableItem : ConsumableItem
{
    public InstantConsumableType type;

    public override void Apply()
    {
        GameItemManager.Instance.AddInstantConsumable(this);
    }
}   

public enum TimedConsumableType
{
    StrengthBoost,
    AttackSpeedBoost
}

public abstract class TimedConsumableItem : ConsumableItem
{
    public TimedConsumableType type;
    public float duration;

    public override void Apply()
    {
        GameItemManager.Instance.AddTimedConsumable(this);
    }
    public abstract void Revert();
}
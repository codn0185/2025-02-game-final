// 자원 아이템 (돈, 특수 재화 등)
public enum ResourceType
{
    Gold,
    Gem
}

public abstract class ResourceItem : ItemBase
{
    public ResourceType type;
    public int amount;

    public override void Apply()
    {
        // 자원 추가 로직 구현
        // PlayerInventory.AddResource(type, amount);
    }
}
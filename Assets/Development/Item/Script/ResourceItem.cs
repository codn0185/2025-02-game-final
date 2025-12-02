// 자원 아이템 (돈, 특수 재화 등)
using UnityEngine;

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
        GameItemManager.Instance.AddResource(this);
    }
}

public class GoldItem : ResourceItem
{
    private void Awake()
    {
        type = ResourceType.Gold;
    }
}

public class GemItem : ResourceItem
{
    private void Awake()
    {
        type = ResourceType.Gem;
    }
}
using UnityEngine;


public enum MagicType
{
    Earth, // 공속 증가, 넉백 증가
    Ice, // 공속 증가, 감속 효과 증가
    Fire, // 공격력 증가, 폭발 범위 증가
    Lighting, // 공격력 증가, 연쇄 개수 증가
}
[CreateAssetMenu(fileName = "Weapon", menuName = "Settings/Weapon")]

public class WeaponStats : ScriptableObject
{
    public MagicType magicType;
    public float projectileSpeed = 10;
    public float attack_speed = 10;
    public float life_time = 10;
    public int damage;
    public bool isKnockback = false;
    public float knockbackPower = 1;
    // Slow
    public bool isSlow = false;
    public int slowPower = 1;
    public float slowDuration = 1.0f;
    // Explosion
    public bool isExplosive = false;
    public float explosionSize = 3f;
    public float lingerDuration = 0f;
    // Chain
    public bool isChain = false;
    public float chainSize = 3;
    public int chainCount = 3;
}
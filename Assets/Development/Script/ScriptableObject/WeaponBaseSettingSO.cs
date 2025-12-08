using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBases", menuName = "Settings/WeaponBase")]
public class WeaponBaseSettingSO : ScriptableObject
{
    public Weapon[] weapons;
}


[CreateAssetMenu(fileName = "Weapon", menuName = "Settings/Weapon")]

public class Weapon : ScriptableObject
{
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
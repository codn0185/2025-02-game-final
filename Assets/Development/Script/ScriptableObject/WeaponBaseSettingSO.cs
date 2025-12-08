using UnityEngine;

public enum MagicType
{
    Earth, // 공속 증가, 넉백 증가
    Ice, // 공속 증가, 감속 효과 증가
    Fire, // 공격력 증가, 폭발 범위 증가
    Lighting, // 공격력 증가, 연쇄 개수 증가
}

[CreateAssetMenu(fileName = "WeaponBases", menuName = "Settings/WeaponBase")]
public class WeaponBaseSettingSO : ScriptableObject
{
    public WeaponStats[] weapons;
}


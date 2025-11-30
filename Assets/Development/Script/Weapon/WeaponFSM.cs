public class WeaponFSM : FSM<WeaponState>
{
    public WeaponDisabledState DisabledState { get; private set; }
    public WeaponCooldownState CooldownState { get; private set; }
    public WeaponReadyState ReadyState { get; private set; }
    public WeaponAttackingState AttackingState { get; private set; }

    public WeaponFSM(WeaponController weapon)
    {
        DisabledState = new WeaponDisabledState(weapon);
        CooldownState = new WeaponCooldownState(weapon);
        ReadyState = new WeaponReadyState(weapon);
        AttackingState = new WeaponAttackingState(weapon);
    }
}
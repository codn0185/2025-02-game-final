public enum WeaponStateType
{
    Disabled, // 무기 비활성화
    Cooldown, // 쿨다운 중
    Ready, // 공격 준비
    Attacking, // 공격 중
}

public abstract class WeaponState : State
{
    protected WeaponController weapon;
    public WeaponStateType StateType { get; protected set; }

    public WeaponState(WeaponController weapon)
    {
        this.weapon = weapon;
    }
}

public class WeaponDisabledState : WeaponState
{
    public WeaponDisabledState(WeaponController weapon) : base(weapon)
    {
        StateType = WeaponStateType.Disabled;
    }

    public override void Enter()
    {
        weapon.SetActive(false);
    }

    public override void Update()
    {
        if (weapon.IsEnabled())
            weapon.StateMachine.ChangeState(weapon.StateMachine.CooldownState);
    }

    public override void Exit()
    {
        weapon.SetActive(true);
    }
}

public class WeaponCooldownState : WeaponState
{
    public WeaponCooldownState(WeaponController weapon) : base(weapon)
    {
        StateType = WeaponStateType.Cooldown;
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (!weapon.IsEnabled())
            weapon.StateMachine.ChangeState(weapon.StateMachine.DisabledState);
        else if (weapon.IsReady())
            weapon.StateMachine.ChangeState(weapon.StateMachine.ReadyState);
    }

    public override void Exit()
    {
    }
}

public class WeaponReadyState : WeaponState
{
    public WeaponReadyState(WeaponController weapon) : base(weapon)
    {
        StateType = WeaponStateType.Ready;
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (!weapon.IsEnabled())
            weapon.StateMachine.ChangeState(weapon.StateMachine.DisabledState);
        else if (weapon.IsReadyToAttack())
            weapon.StateMachine.ChangeState(weapon.StateMachine.AttackingState);
    }

    public override void Exit()
    {
    }
}

public class WeaponAttackingState : WeaponState
{
    public WeaponAttackingState(WeaponController weapon) : base(weapon)
    {
        StateType = WeaponStateType.Attacking;
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (!weapon.IsEnabled())
            weapon.StateMachine.ChangeState(weapon.StateMachine.DisabledState);
        else if (weapon.IsAttackFinished())
            weapon.StateMachine.ChangeState(weapon.StateMachine.CooldownState);
    }

    public override void Exit()
    {
    }
}
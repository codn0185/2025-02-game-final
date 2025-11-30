public enum PlayerStateType
{
    Idle, // 정지
    Move, // 이동 중
    Dead // 사망
}

public static class PlayerAnimatorParameter
{
    public const string Idle = "Idle"; // boolean
    public const string Move = "Move"; // boolean
    public const string Dead = "Dead"; // boolean
    public const string Attack = "Attack"; // trigger
    public const string Hit = "Hit"; // trigger
}

public abstract class PlayerState : State
{
    protected PlayerController player;
    public PlayerStateType StateType { get; protected set; }

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }
}

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player) : base(player)
    {
        StateType = PlayerStateType.Idle;
    }

    public override void Enter()
    {
        player.Animator.SetBool(PlayerAnimatorParameter.Idle, true);
        player.StartAttackCoroutine();
    }

    public override void Update()
    {
        if (player.IsDead())
            player.StateMachine.ChangeState(player.StateMachine.DeadState);
        else if (player.HasMovementInput())
            player.StateMachine.ChangeState(player.StateMachine.MoveState);
    }

    public override void Exit()
    {
        player.Animator.SetBool(PlayerAnimatorParameter.Idle, false);
    }
}

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController player) : base(player)
    {
        StateType = PlayerStateType.Move;
    }

    public override void Enter()
    {
        player.Animator.SetBool(PlayerAnimatorParameter.Move, true);
    }

    public override void Update()
    {
        if (player.IsDead())
            player.StateMachine.ChangeState(player.StateMachine.DeadState);
        else if (!player.HasMovementInput())
            player.StateMachine.ChangeState(player.StateMachine.IdleState);
        else
            player.Move();
    }

    public override void Exit()
    {
        player.Animator.SetBool(PlayerAnimatorParameter.Move, false);
    }
}

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerController player) : base(player)
    {
        StateType = PlayerStateType.Dead;
    }

    public override void Enter()
    {
        player.Animator.SetBool(PlayerAnimatorParameter.Dead, true);
        player.StopAttackCoroutine();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        player.Animator.SetBool(PlayerAnimatorParameter.Dead, false);
    }
}
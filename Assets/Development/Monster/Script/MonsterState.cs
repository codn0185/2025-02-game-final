public static class MonsterAnimatorParameter
{
    public static string Idle = "Idle"; // boolean
    public static string Move = "Move"; // boolean
    public static string Battle = "Battle"; // boolean
    public static string Dead = "Dead"; // boolean
    public static string Attack = "Attack"; // trigger
    public static string Hit = "Hit"; // trigger
}

public abstract class MonsterState : State
{
    protected MonsterController monster;

    public MonsterState(MonsterController monster)
    {
        this.monster = monster;
    }
}

public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(MonsterController monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Idle, true);
    }

    public override void Update()
    {
        if (monster.IsDead())
            monster.StateMachine.ChangeState(monster.StateMachine.DeadState);
    }

    public override void Exit()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Idle, false);
    }
}
    
public class MonsterMoveState : MonsterState
{
    public MonsterMoveState(MonsterController monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Move, true);
    }

    public override void Update()
    {
        if (monster.IsDead())
            monster.StateMachine.ChangeState(monster.StateMachine.DeadState);
        // else if (monster.ReachedAttackPoint())
        //     monster.StateMachine.ChangeState(monster.StateMachine.BattleState);
        else
            monster.Move();
    }

    public override void Exit()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Move, false);
    }
}

public class MonsterBattleState : MonsterState
{
    public MonsterBattleState(MonsterController monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Battle, true);
        monster.StartAttackCoroutine();
    }

    public override void Update()
    {
        if (monster.IsDead())
            monster.StateMachine.ChangeState(monster.StateMachine.DeadState);
        // else if (!monster.ReachedAttackPoint())
        //     monster.StateMachine.ChangeState(monster.StateMachine.MoveState);
    }

    public override void Exit()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Battle, false);
        monster.StopAttackCoroutine();
    }
}

public class MonsterDeadState : MonsterState
{
    public MonsterDeadState(MonsterController monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Dead, true);
        monster.Die();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        monster.Animator.SetBool(MonsterAnimatorParameter.Dead, false);
    }
}
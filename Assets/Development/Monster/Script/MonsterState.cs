public enum MonsterStateType
{
    Idle, // 정지
    Move, // 이동 중
    Battle, // 전투 중
    Dead // 사망
}

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
    public MonsterStateType StateType { get; protected set; }

    public MonsterState(MonsterController monster)
    {
        this.monster = monster;
    }
}

public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(MonsterController monster) : base(monster)
    {
        StateType = MonsterStateType.Idle;
    }

    public override void Enter()
    {
        monster.OnEnterIdle();
    }

    public override void Update()
    {
        monster.OnUpdateIdle();
    }

    public override void Exit()
    {
        monster.OnExitIdle();
    }
}
    
public class MonsterMoveState : MonsterState
{
    public MonsterMoveState(MonsterController monster) : base(monster)
    {
        StateType = MonsterStateType.Move;
    }

    public override void Enter()
    {
        monster.OnEnterMove();
    }

    public override void Update()
    {
        monster.OnUpdateMove();
    }

    public override void Exit()
    {
        monster.OnExitMove();
    }
}

public class MonsterBattleState : MonsterState
{
    public MonsterBattleState(MonsterController monster) : base(monster)
    {
        StateType = MonsterStateType.Battle;
    }

    public override void Enter()
    {
        monster.OnEnterBattle();
    }

    public override void Update()
    {
        monster.OnUpdateBattle();
    }

    public override void Exit()
    {
        monster.OnExitBattle();
    }
}

public class MonsterDeadState : MonsterState
{
    public MonsterDeadState(MonsterController monster) : base(monster)
    {
        StateType = MonsterStateType.Dead;
    }

    public override void Enter()
    {
        monster.OnEnterDead();
    }

    public override void Update()
    {
        monster.OnUpdateDead();
    }

    public override void Exit()
    {
        monster.OnExitDead();
    }
}
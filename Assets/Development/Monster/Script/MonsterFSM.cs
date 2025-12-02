public class MonsterFSM : FSM<MonsterState>
{
    public MonsterIdleState IdleState { get; private set; }
    public MonsterMoveState MoveState { get; private set; }
    public MonsterBattleState BattleState { get; private set; }
    public MonsterDeadState DeadState { get; private set; }

    public MonsterFSM(MonsterController monster)
    {
        IdleState = new MonsterIdleState(monster);
        MoveState = new MonsterMoveState(monster);
        BattleState = new MonsterBattleState(monster);
        DeadState = new MonsterDeadState(monster);
    }
}
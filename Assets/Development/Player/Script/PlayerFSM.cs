public class PlayerFSM : FSM<PlayerState>
{
    public PlayerIdleState IdleState { get; set; }
    public PlayerMoveState MoveState { get; set; }
    public PlayerDeadState DeadState { get; set; }

    public PlayerFSM(PlayerController player)
    {
        IdleState = new PlayerIdleState(player);
        MoveState = new PlayerMoveState(player);
        DeadState = new PlayerDeadState(player);
    }
}
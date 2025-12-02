public abstract class FSM<T> where T : State
{
    public T CurrentState { get; protected set; }

    public void Initialize(T initialState)
    {
        CurrentState = initialState;
        CurrentState?.Enter();
    }

    public void ChangeState(T newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
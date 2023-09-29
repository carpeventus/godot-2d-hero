

public class StateMachine<T> where T : IStateMachineState {
    public T CurrentState { get; private set; }
    public T PreviousState { get; private set; }
    
    public void InitState(T initState) {
        CurrentState = initState;
        CurrentState.OnEnter();
    }

    public void ChangeState(T newState) {
        CurrentState.OnExit();
        PreviousState = CurrentState;
        CurrentState = newState;
        CurrentState.OnEnter();
    }
}
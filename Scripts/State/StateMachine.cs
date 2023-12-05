using System.Collections.Generic;

public class StateMachine<T> where T : IStateMachineState {
    public T CurrentState { get; private set; }
    public T PreviousState { get; private set; }
    
    public void InitState(T initState) {
        initState.OnEnter();
        CurrentState = initState;
    }

    public void ChangeState(T newState) {
        // 不允许重复进入同一个状态
        if (EqualityComparer<T>.Default.Equals(newState, CurrentState))
        {
            return;
        }
        CurrentState.OnExit();
        newState.OnEnter();
        PreviousState = CurrentState;
        CurrentState = newState;
    }
}
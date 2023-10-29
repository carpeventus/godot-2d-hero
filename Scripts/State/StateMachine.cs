

using System.Collections.Generic;

public class StateMachine<T> where T : IStateMachineState {
    public T CurrentState { get; private set; }
    public T PreviousState { get; private set; }
    
    public void InitState(T initState) {
        CurrentState = initState;
        CurrentState.OnEnter();
    }

    public void ChangeState(T newState) {
        // 不允许重复进入同一个状态
        if (EqualityComparer<T>.Default.Equals(newState, CurrentState))
        {
            return;
        }
        CurrentState.OnExit();
        PreviousState = CurrentState;
        CurrentState = newState;
        CurrentState.OnEnter();
    }
}
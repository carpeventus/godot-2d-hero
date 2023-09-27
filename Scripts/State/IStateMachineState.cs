
public interface IStateMachineState { 
    void OnEnter();
    void OnExit();
    void PhysicsUpdate(double delta);
    void LogicUpdate(double delta);
}
using Godot;

public class BoarState : IStateMachineState {
    protected Boar boar;
    protected StateMachine<BoarState> stateMachine;
    protected string animationName;
    
    public BoarState(StateMachine<BoarState> stateMachine, Boar boar, string animationName) {
        this.stateMachine = stateMachine;
        this.boar = boar;
        this.animationName = animationName;
    }
    
    protected void BasicChangeVelocity(double delta, float speed, float hAcceleration) {
        Vector2 velocity = boar.Velocity;
        velocity.Y += boar.DefaultGravity * (float)delta;
        velocity.X = (float) Mathf.MoveToward(boar.Velocity.X, speed * boar.Direction, hAcceleration * delta);
        boar.Velocity = velocity;
    }

    
    public virtual void OnEnter() {
        boar.AnimationPlayer.Play(animationName);
    }

    public virtual void OnExit() {
    }

    public virtual void PhysicsUpdate(double delta) {
    }

    public virtual void LogicUpdate(double delta) {
    }
}
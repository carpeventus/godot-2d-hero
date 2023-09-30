using Godot;

public class BoarWalkState : BoarState {
    public BoarWalkState(StateMachine<BoarState> stateMachine, Boar boar, string animationName) : base(stateMachine, boar, animationName) {
    }


    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta); 
        if (boar.PlayerCheck.IsColliding()) {
            stateMachine.ChangeState(boar.BoarRunState);
        }
        if (boar.CanNotMoveTowards()) {
            stateMachine.ChangeState(boar.BoarIdleState);
        }
    }
    
    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        BasicChangeVelocity(delta, boar.MoveSpeed, boar.Acceleration);
        boar.MoveAndSlide();
    }
}
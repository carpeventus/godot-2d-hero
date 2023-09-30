
public class BoarRunState : BoarState{
    public BoarRunState(StateMachine<BoarState> stateMachine, Boar boar, string animationName) : base(stateMachine, boar, animationName) {
    }
    
    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        BasicChangeVelocity(delta, boar.MaxSpeed, boar.Acceleration);
        boar.MoveAndSlide();
    }
    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (boar.PlayerCheck.IsColliding()) {
            boar.DelayStopRunTimer.Start();
        }

        // 立即转身
        if (boar.CanNotMoveTowards()) {
            boar.Flip();
        }
        
        if (boar.DelayStopRunTimer.IsStopped()) {
            stateMachine.ChangeState(boar.BoarWalkState);
        }
    }
}
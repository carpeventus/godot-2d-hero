using Godot;

public class BoarIdleState : BoarState {
    public BoarIdleState(StateMachine<BoarState> stateMachine, Boar boar, string animationName) : base(stateMachine, boar, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        DelayToNextState();
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (boar.PlayerCheck.IsColliding()) {
            stateMachine.ChangeState(boar.BoarRunState);
        }
    }

    private async void DelayToNextState() {
        await boar.ToSignal(boar.GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
        if (boar.CanNotMoveTowards()) {
            boar.Flip();
        }
        stateMachine.ChangeState(boar.BoarWalkState);
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        BasicChangeVelocity(delta, 0, boar.Acceleration);
        boar.MoveAndSlide();
    }
}
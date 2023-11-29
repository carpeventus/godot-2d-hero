using Godot;

public class PlayerSlideLoopState : PlayerOnFloorState {
    public PlayerSlideLoopState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        Sliding();
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        player.Slide(delta);
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (player.IsOnWall()) {
            stateMachine.ChangeState(player.PlayerSlideEndState);
        }
    }

    private async void Sliding() {
        await player.ToSignal(player.GetTree().CreateTimer(0.3), SceneTreeTimer.SignalName.Timeout);
        stateMachine.ChangeState(player.PlayerSlideEndState);
    }
}
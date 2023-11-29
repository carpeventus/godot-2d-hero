using Godot;

public class PlayerSlideEndState : PlayerOnFloorState {
    public PlayerSlideEndState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    
    
    public override void OnEnter() {
        base.OnEnter();
        WaitForAnimationFinished();
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        Vector2 velocity = player.Velocity;
        velocity.Y += player.DefaultGravity * (float)delta;
        velocity.X = 0f;
        player.Velocity = velocity;
        player.MoveAndSlide();
    }

    private async void WaitForAnimationFinished() {
        await player.ToSignal(player.AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        stateMachine.ChangeState(player.PlayerIdleState);
    }
}
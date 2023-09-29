using Godot;

public class PlayerWallSlidingState : PlayerTouchingWallState {
    public PlayerWallSlidingState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    public override void OnEnter() {
        base.OnEnter();
        player.Velocity = new Vector2(player.Velocity.X, 0f);
        player.CurrentGravity = player.DefaultGravity / 10f;
    }

    public override void OnExit() {
        base.OnExit();
        player.CurrentGravity = player.DefaultGravity;
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        BasicChangeVelocity(delta, player.CurrentGravity, player.InAirAcceleration);
        player.MoveAndSlide();
    }
}
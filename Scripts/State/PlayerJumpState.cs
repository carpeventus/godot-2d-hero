using Godot;


public class PlayerJumpState : PlayerState {
    public PlayerJumpState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        player.Velocity = new Vector2(player.Velocity.X, 0f);
        player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
        player.MoveAndSlide();
        stateMachine.ChangeState(player.PlayerInAirState);
    }

    public override void OnExit() {
        base.OnExit();
        player.CoyoteTimer.Stop();
        player.JumpDelayInputTimer.Stop();
    }
}
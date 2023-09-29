using Godot;


public class PlayerLandingState : PlayerOnFloorState {
    public PlayerLandingState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!Mathf.IsZeroApprox(player.InputDirection)) {
            stateMachine.ChangeState(player.PlayerRunState);
        } else {
            if (!player.AnimationPlayer.IsPlaying()) {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
        }
    }
}
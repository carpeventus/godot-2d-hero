using Godot;


public class PlayerRunState : PlayerOnFloorState {
    public PlayerRunState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (player.ShouldSlide()) {
            stateMachine.ChangeState(player.PlayerSlideStartState);
        }

        if (Mathf.IsZeroApprox(player.InputDirection)) {
            stateMachine.ChangeState(player.PlayerIdleState);
        }
    }
}
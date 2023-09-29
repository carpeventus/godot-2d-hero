using Godot;

public class PlayerIdleState : PlayerOnFloorState {
    public PlayerIdleState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!Mathf.IsZeroApprox(player.InputDirection)) {
            stateMachine.ChangeState(player.PlayerRunState);
        }
    }
}
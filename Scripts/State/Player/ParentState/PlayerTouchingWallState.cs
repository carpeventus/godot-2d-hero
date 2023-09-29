using Godot;

public class PlayerTouchingWallState : PlayerState {
    public PlayerTouchingWallState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (player.IsOnFloor()) {
            stateMachine.ChangeState(player.PlayerIdleState);
        }

        if (player.JumpDelayInputTimer.TimeLeft > 0 && player.IsCollidingFront()) {
            stateMachine.ChangeState(player.PlayerWallJumpState);
        }
        // 不在墙上或没有输入
        if (!player.IsCollidingFront() || Mathf.IsZeroApprox(player.InputDirection)) {
            stateMachine.ChangeState(player.PlayerInAirState);
        }
        

        
    }
}
using Godot;

public class PlayerOnFloorState : PlayerState {
    public PlayerOnFloorState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        // 上一个状态不是地面状态, 刚刚着陆
        if (stateMachine.PreviousState is not PlayerOnFloorState) {
            player.CoyoteTimer.Stop();
        }
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        BasicChangeVelocity(delta, player.CurrentGravity, player.MoveAcceleration);
        player.MoveAndSlide();
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (player.IsDead())
        { 
            stateMachine.ChangeState(player.PlayerDieState);
        }
        if (!Mathf.IsZeroApprox(player.InputDirection)) {
            player.FlipSprite(player.InputDirection < 0);
        }
        
        if (player.ShouldJump()) {
            stateMachine.ChangeState(player.PlayerJumpState);
        }
        
        if (!player.IsOnFloor()) {
            stateMachine.ChangeState(player.PlayerInAirState);
        }
        
        if (Input.IsActionJustPressed("attack")) {
            stateMachine.ChangeState(player.PlayerAttack1State);
        }

    }
}
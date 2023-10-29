using Godot;

public class PlayerAttackState : PlayerState {
    public PlayerAttackState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        player.IsAttackComboRequested = false;
    }

    public override void OnExit() {
        base.OnExit();
        player.IsAttackComboRequested = false;
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        Vector2 velocity = player.Velocity;
        velocity.Y += player.DefaultGravity * (float)delta;
        velocity.X = 0f;
        player.Velocity = velocity;
        player.MoveAndSlide();
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!player.IsOnFloor()) {
            stateMachine.ChangeState(player.PlayerInAirState);
        }
        // 跳跃取消攻击后摇
        if (shouldJump) {
            stateMachine.ChangeState(player.PlayerJumpState);
        }
    }
    
    
}
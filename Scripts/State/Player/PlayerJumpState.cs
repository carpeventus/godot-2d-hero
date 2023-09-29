using Godot;
using Hero.Scripts.State.Player.ParentState;


public class PlayerJumpState : PlayerAbility {
    public PlayerJumpState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        // 动画实际上不会播放，因为会使跳跃操作手感稀碎
        base.OnEnter();
        player.Velocity = new Vector2(player.Velocity.X, 0f);
        player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
        IsAbilityDone = true;
    }

    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        player.MoveAndSlide();
    }

    public override void OnExit() {
        base.OnExit();
        player.CoyoteTimer.Stop();
        player.JumpDelayInputTimer.Stop();
    }
}
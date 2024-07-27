using Godot;


public class PlayerJumpState : PlayerAbility {
    public PlayerJumpState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        // 动画实际上不会播放，立刻就转到因为会使跳跃操作手感稀碎
        base.OnEnter();
        player.SoundManager.PlayerSfx("Jump");
        player.Velocity = new Vector2(player.Velocity.X, 0f);
        player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
        player.MoveAndSlide();
        IsAbilityDone = true;
    }

    public override void OnExit() {
        base.OnExit();
        player.CoyoteTimer.Stop();
        player.JumpDelayInputTimer.Stop();
    }
}
using Godot;
using Hero.Scripts.State.Player.ParentState;

namespace Hero.Scripts.State.Player; 

public class PlayerWallJumpState : PlayerAbility {
    public PlayerWallJumpState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        player.Velocity = Vector2.Zero;
        float velocityX = player.GetWallNormal().X < 0 ? -player.WallJumpVelocity.X : player.WallJumpVelocity.X;
        player.Velocity = new Vector2(velocityX, player.WallJumpVelocity.Y);
        IsAbilityDone = true;
    }
    
    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        player.MoveAndSlide();
    }


    public override void OnExit() {
        base.OnExit();
        player.JumpDelayInputTimer.Stop();
    }
}
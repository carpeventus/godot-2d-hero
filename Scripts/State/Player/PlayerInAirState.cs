using System.Diagnostics;
using Godot;

public class PlayerInAirState : PlayerState {
    public PlayerInAirState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    
    
    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        ModifyGravityInAir();
        BasicChangeVelocity(delta, player.CurrentGravity, player.InAirAcceleration);
        player.MoveAndSlide();
    }

    public override void OnEnter() {
        base.OnEnter();
        // 从地面落下
        if (stateMachine.PreviousState is PlayerOnFloorState && player.Velocity.Y > 0) {
            player.CoyoteTimer.Start();
        }
    }

    public override void OnExit() {
        base.OnExit();
        player.CurrentGravity = player.DefaultGravity;
    }

    public void ModifyGravityInAir() {
        player.CurrentGravity = player.DefaultGravity;
        // 下落
        if (player.Velocity.Y > 0) {
            player.CurrentGravity = player.DefaultGravity * (3* player.FallMulti / 4);
        }
        // 上升中
        else if (player.Velocity.Y < 0 && !Input.IsActionPressed("jump")) {
            player.CurrentGravity = player.DefaultGravity * (player.FallMulti / 2);
        }
        
    }
    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!Mathf.IsZeroApprox(player.Velocity.X)) {
            player.FlipSprite(player.Velocity.X < 0);
        }

        if (player.Velocity.Y > 0) {
            player.AnimationPlayer.Play("player_falling");
        }

        // 这里没有判断IsOnWall，贴墙跳起来IsOnWall始终为false，可能是跳跃动画的碰撞体真的没有和墙壁进行碰撞。这里仅使用RayCast即可
        if (player.JumpDelayInputTimer.TimeLeft > 0 && player.IsCollidingFront()) {
            stateMachine.ChangeState(player.PlayerWallJumpState);
        }

        
        if (player.IsOnFloor()) {
            stateMachine.ChangeState(player.PlayerLandingState);
        }

        // 玩家在垂直方向Velocity大于0才认为是下滑
        if (player.IsCollidingFront() && player.Velocity.Y > player.WallSlideThreshold) {
            if (player.IsWallDirectionEqualsInputDirection()) {
                stateMachine.ChangeState(player.PlayerWallSlidingState);
            }
        }
    }
}
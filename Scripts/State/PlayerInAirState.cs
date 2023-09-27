using Godot;

public class PlayerInAirState : PlayerState {
    public PlayerInAirState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    
    
    public override void PhysicsUpdate(double delta) {
        base.PhysicsUpdate(delta);
        ModifyGravityInAir();
        BasicChangeVelocity(delta, player.InAirAcceleration);
        player.MoveAndSlide();
    }

    public override void OnEnter() {
        base.OnEnter();
        // 从地面落下
        if (stateMachine.PreviousState is PlayerOnFloorState && player.Velocity.Y > 0) {
            player.CoyoteTimer.Start();
        }
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
        if (!Mathf.IsZeroApprox(player.Direction)) {
            player.FlipSprite(player.Direction < 0);
        }

        if (player.Velocity.Y > 0) {
            player.AnimationPlayer.Play("player_falling");
        }

        if (player.IsOnFloor()) {
            stateMachine.ChangeState(player.PlayerLandingState);
        }
    }
}
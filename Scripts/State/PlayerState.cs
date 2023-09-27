using Godot;

public class PlayerState : IStateMachineState {

    protected PlayerController player;
    protected StateMachine<PlayerState> stateMachine;
    protected string animationName;
    protected bool shouldJump;
    
    public PlayerState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animationName = animationName;
    }

    public virtual void OnEnter() {
        player.AnimationPlayer.Play(animationName);
    }

    public virtual void OnExit() {
    }

    public virtual void PhysicsUpdate(double delta) {
    }

    protected void BasicChangeVelocity(double delta, float acceleration) {
        Vector2 velocity = player.Velocity;
        velocity.Y += player.CurrentGravity * (float)delta;
        velocity.X = (float) Mathf.MoveToward(player.Velocity.X, player.Direction * player.MoveSpeed, acceleration * delta);
        player.Velocity = velocity;
    }

    public virtual void LogicUpdate(double delta) {
        player.Direction = Input.GetAxis("move_left", "move_right");
        shouldJump = (player.IsOnFloor() || player.CoyoteTimer.TimeLeft > 0) && player.JumpDelayInputTimer.TimeLeft > 0;
    }


}
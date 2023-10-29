using Godot;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        player.Velocity = Vector2.Zero;
        player.MoveAndSlide();
    }
}
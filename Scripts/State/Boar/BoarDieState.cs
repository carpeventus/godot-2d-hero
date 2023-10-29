using Godot;

public class BoarDieState : BoarState
{
    public BoarDieState(StateMachine<BoarState> stateMachine, Boar boar, string animationName) : base(stateMachine, boar, animationName)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        boar.Velocity = Vector2.Zero;
        boar.MoveAndSlide();
    }
}
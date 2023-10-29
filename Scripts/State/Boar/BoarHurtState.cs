
using Godot;

public class BoarHurtState : BoarState
{
    public BoarHurtState(StateMachine<BoarState> stateMachine, Boar boar, string animationName) : base(stateMachine, boar, animationName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        var attackDirection = Mathf.Sign(boar.GlobalPosition.X - boar.CurrentTakenDamage.source.GlobalPosition.X) == 1 ? 1 : -1;
        // 攻击方向-1，说明要朝右；攻击方向1，攻击源在左右，要朝左
        if (attackDirection == boar.Direction)
        {
            boar.Flip();
        }
        
        var velocity = Vector2.Zero;
        velocity.X = boar.KnockBackForce * attackDirection;
        boar.TakeDamage();
        boar.Velocity = velocity;
        WaitHurtFinished();
    }
    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);
        boar.MoveAndSlide();
    }
    private async void WaitHurtFinished()
    {
        await boar.ToSignal(boar.AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        // 已经死亡，就直接返回 
        if (stateMachine.CurrentState == boar.BoarDieState)
        {
            return;
        }
        // 被攻击，转到奔跑
        stateMachine.ChangeState(boar.BoarRunState);
    }
}
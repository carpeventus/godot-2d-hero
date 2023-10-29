using System;
using Godot;

public class PlayerHurtState : PlayerState
{
    public PlayerHurtState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        var attackDirection = Mathf.Sign(player.GlobalPosition.X - player.CurrentTakenDamage.source.GlobalPosition.X) == 1 ? 1 : -1;
        var velocity = Vector2.Zero;
        velocity.X = player.KnockBackForce * attackDirection;
        player.TakeDamage();
        player.Velocity = velocity;
        WaitHurtFinished();
    }

    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);
        player.MoveAndSlide();
    }

    private async void WaitHurtFinished()
    {
        await player.ToSignal(player.AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        // 已经死亡，就直接返回 
        if (stateMachine.CurrentState == player.PlayerDieState)
        {
            return;
        }
        // 先转到Idle，再从这个状态自动转到其他状态
        stateMachine.ChangeState(player.PlayerIdleState);
    }
}
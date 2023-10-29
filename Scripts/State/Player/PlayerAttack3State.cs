
public class PlayerAttack3State : PlayerAttackState {
    public PlayerAttack3State(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    

    
    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!player.AnimationPlayer.IsPlaying()) {
            stateMachine.ChangeState(player.PlayerIdleState);
        }
    }
}
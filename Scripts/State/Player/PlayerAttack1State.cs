using Hero.Scripts.State.Player.ParentState;

namespace Hero.Scripts.State.Player; 

public class PlayerAttack1State : PlayerAttackState {
    public PlayerAttack1State(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!player.AnimationPlayer.IsPlaying()) {
            if (player.IsAttackComboRequested) {
                stateMachine.ChangeState(player.PlayerAttack2State);
            }
            else {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
        }
    }
}
namespace Hero.Scripts.State.Player.ParentState; 

public class PlayerAbility : PlayerState {
    protected bool IsAbilityDone;
    
    public PlayerAbility(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        IsAbilityDone = false;
    }

    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (IsAbilityDone) {
            if (player.IsOnFloor()) {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
            else {
                stateMachine.ChangeState(player.PlayerInAirState);
            }
        }
    }
}
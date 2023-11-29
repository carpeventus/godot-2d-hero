using Godot;

public class PlayerSlideStartState : PlayerOnFloorState {
    public PlayerSlideStartState(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }

    public override void OnEnter() {
        base.OnEnter();
        WaitForAnimationFinished();
    }

    private async void WaitForAnimationFinished() {
        await player.ToSignal(player.AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        stateMachine.ChangeState(player.PlayerSlideLoopState);
    }
}
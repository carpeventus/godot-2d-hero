using Hero.Scripts.State.Player.ParentState;

namespace Hero.Scripts.State.Player; 

public class PlayerAttack2State : PlayerAttackState {
    public PlayerAttack2State(StateMachine<PlayerState> stateMachine, PlayerController player, string animationName) : base(stateMachine, player, animationName) {
    }
    
    public override void LogicUpdate(double delta) {
        base.LogicUpdate(delta);
        if (!player.AnimationPlayer.IsPlaying()) {
            // 这里没有使用Input.IsActionPressed实时判断是否按下跳跃，因为我们是等动画播放完了再判断的（相当于是等攻击后摇结束），不好卡那一帧。
            // 这里的实现是在动画canCombo = true 期间都能将IsAttackComboRequested设置为true，那么在动画播放完后能立即转换到下一段攻击
            if (player.IsAttackComboRequested) {
                stateMachine.ChangeState(player.PlayerAttack3State);
            }
            else {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
        }
    }
}
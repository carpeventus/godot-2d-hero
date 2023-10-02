using Godot;
using System;

[GlobalClass]
public partial class HitBox : Area2D
{
    
    [Signal]
    public delegate void HitEventHandler(HurtBox hurtBox);
    
    public override void _Ready() {
        base._Ready();
        AreaEntered += OnAreaEntered;
    }
    
    public void OnAreaEntered(Area2D area) {
        HurtBox hurtBox = area as HurtBox;
        EmitSignal(SignalName.Hit, hurtBox);
        hurtBox?.EmitSignal(HurtBox.SignalName.Hurt, this);
    }
}

using Godot;
using System;

[GlobalClass]
public partial class HitBox : Area2D
{
    
    [Signal]
    public delegate void HitEventHandler(HurtBox hurtBox);

    private GameGlobal _gameGlobal; 
    public override void _Ready()
    {
        _gameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
        base._Ready();
        AreaEntered += OnAreaEntered;
    }
    
    public void OnAreaEntered(Area2D area) {
        HurtBox hurtBox = area as HurtBox;
        EmitSignal(SignalName.Hit, hurtBox);
        hurtBox?.EmitSignal(HurtBox.SignalName.Hurt, this);
        Shake();
    }

    public async void Shake()
    {
        _gameGlobal.ShakeCamera(8, 0.8f);
        Engine.TimeScale = 0.1;
        var timer = GetTree().CreateTimer(0.1, true, false, true);

        await ToSignal(timer, Timer.SignalName.Timeout);
        Engine.TimeScale = 1;
    }
}

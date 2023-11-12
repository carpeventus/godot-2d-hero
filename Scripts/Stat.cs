using Godot;
using System;

public partial class Stat : Node
{
    [Export] public int MaxHealth;

    [Signal]
    public delegate void HealthChangedEventHandler();

    public int CurrentHealth { get; private set; }

    public void TakeDamage(int damage) {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        EmitSignal(SignalName.HealthChanged);
    }

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }
}

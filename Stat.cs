using Godot;
using System;

public partial class Stat : Node
{
    [Export] public int MaxHealth;

    public int CurrentHealth { get; set; }

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }
}

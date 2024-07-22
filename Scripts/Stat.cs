using Godot;
using System;

[GlobalClass]
public partial class Stat : Node
{
	[Export] public int MaxHealth = 3;
	[Export] public float MaxEnergy = 10f;
	[Export] public float EnergyReGenSpeed = 0.8f;

	[Signal]
	public delegate void HealthChangedEventHandler();
	
	[Signal]
	public delegate void EnergyChangedEventHandler();

	public int CurrentHealth { get; set; }
	public float CurrentEnergy { get;  set; }

	public void TakeDamage(int damage) {
		CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
		EmitSignal(SignalName.HealthChanged);
	}

	public override void _Ready()
	{
		ResetHealthAndEnergy();
	}

	public void ResetHealthAndEnergy()
	{
		
		CurrentHealth = MaxHealth;
		CurrentEnergy = MaxEnergy;
		EmitSignal(SignalName.HealthChanged);
		EmitSignal(SignalName.EnergyChanged);
	}

	public override void _Process(double delta) {
		var before = CurrentEnergy;
		CurrentEnergy = Mathf.Clamp(before + EnergyReGenSpeed * (float)delta, 0f, MaxEnergy);
		if (!Mathf.IsZeroApprox(CurrentEnergy - before)) {
			EmitSignal(SignalName.EnergyChanged);
		}
	}
}

using Godot;
using System;

public partial class PlayerStarus : MarginContainer {
	[Export] public Stat PlayerStat;
	private TextureProgressBar _healthProgressBar;
	private TextureProgressBar _energyProgressBar;
	private TextureProgressBar _easeHealthProgressBar;
	public override void _Ready() {
		_healthProgressBar = GetNode<TextureProgressBar>("HBoxContainer/VBoxContainer/HealthBar");
		_energyProgressBar = GetNode<TextureProgressBar>("HBoxContainer/VBoxContainer/EnegyBar");
		_easeHealthProgressBar = GetNode<TextureProgressBar>("HBoxContainer/VBoxContainer/HealthBar/EaseHealthBar");
		PlayerStat.HealthChanged += OnHealthChanged;
		PlayerStat.EnergyChanged += OnEnergyChanged;
	}

	private void OnHealthChanged() {
		ShowHealth();
	}
	
	private void OnEnergyChanged() {
		ShowEnergy();
	}

	private void ShowHealth() {
		var percent = (float) PlayerStat.CurrentHealth / PlayerStat.MaxHealth;
		_healthProgressBar.Value = percent;
		var tween = CreateTween();
		tween.TweenProperty(_easeHealthProgressBar, "value", percent, 0.3f);
	}
	
	private void ShowEnergy() {
		var percent = PlayerStat.CurrentEnergy / PlayerStat.MaxEnergy;
		_energyProgressBar.Value = percent;
	}
}

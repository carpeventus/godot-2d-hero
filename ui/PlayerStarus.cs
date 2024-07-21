using Godot;
using System;

public partial class PlayerStarus : MarginContainer {
	private TextureProgressBar _healthProgressBar;
	private TextureProgressBar _energyProgressBar;
	private TextureProgressBar _easeHealthProgressBar;
	public GameGlobal GameGlobal { get; private set; } 

	public override void _Ready() {
		GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
		_healthProgressBar = GetNode<TextureProgressBar>("HBoxContainer/VBoxContainer/HealthBar");
		_energyProgressBar = GetNode<TextureProgressBar>("HBoxContainer/VBoxContainer/EnegyBar");
		_easeHealthProgressBar = GetNode<TextureProgressBar>("HBoxContainer/VBoxContainer/HealthBar/EaseHealthBar");
		GameGlobal.Stat.HealthChanged += OnHealthChanged;
		GameGlobal.Stat.EnergyChanged += OnEnergyChanged;
		ShowEnergy();
		ShowHealth();
	}

	public override void _ExitTree() {
		GameGlobal.Stat.HealthChanged -= OnHealthChanged;
		GameGlobal.Stat.EnergyChanged -= OnEnergyChanged;
	}

	private void OnHealthChanged() {
		ShowHealth();
	}
	
	private void OnEnergyChanged() {
		ShowEnergy();
	}

	private void ShowHealth() {
		var percent = (float) GameGlobal.Stat.CurrentHealth / GameGlobal.Stat.MaxHealth;
		_healthProgressBar.Value = percent;
		var tween = CreateTween();
		tween.TweenProperty(_easeHealthProgressBar, "value", percent, 0.3f);
	}
	
	private void ShowEnergy() {
		var percent = GameGlobal.Stat.CurrentEnergy / GameGlobal.Stat.MaxEnergy;
		_energyProgressBar.Value = percent;
	}
}

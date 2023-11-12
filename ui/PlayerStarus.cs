using Godot;
using System;

public partial class PlayerStarus : MarginContainer {
	[Export] public Stat PlayerStat;
	private TextureProgressBar _healthProgressBar;
	private TextureProgressBar _easeHealthProgressBar;
	public override void _Ready() {
		_healthProgressBar = GetNode<TextureProgressBar>("HBoxContainer/HealthBar");
		_easeHealthProgressBar = GetNode<TextureProgressBar>("HBoxContainer/HealthBar/EaseHealthBar");

		PlayerStat.HealthChanged += OnHealthChanged;
		ShowHealth();
	}

	private void OnHealthChanged() {
		ShowHealth();
	}

	private void ShowHealth() {
		var percent = (float) PlayerStat.CurrentHealth / PlayerStat.MaxHealth;
		_healthProgressBar.Value = percent;
		var tween = CreateTween();
		tween.TweenProperty(_easeHealthProgressBar, "value", percent, 0.3f);
	}
}

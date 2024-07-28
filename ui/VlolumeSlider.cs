using Godot;
using System;

public partial class VlolumeSlider : HSlider
{
	[Export] public string BusName;
	public GameGlobal GameGlobal { get; private set; }
	public SoundManager SoundManager { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SoundManager = GetNode<SoundManager>("/root/SoundManager");
		GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
		Value = SoundManager.GetVolume(BusName);
		
		ValueChanged += value => SaveVolume((float)value);
	}

	public void SaveVolume(float value)
	{
		SoundManager.SetVolume(value, BusName);
		GameGlobal.SaveConfig();
	}
}

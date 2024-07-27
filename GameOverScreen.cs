using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
	[Export] public AudioStream Bgm;
	private GameGlobal _gameGlobal;
	public SoundManager SoundManager { get; private set; }
	public override void _Ready()
	{
		_gameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
		SoundManager = GetNode<SoundManager>("/root/SoundManager");
		SoundManager.PlayerBgm(Bgm);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("enter"))
		{
			GetTree().Paused = false;
			if (_gameGlobal.HasSaveFile())
			{
				_gameGlobal.LoadGame();
			}
			else
			{
				GetTree().ChangeSceneToFile("res://world.tscn");
			}
		}
	}
}

using Godot;
using System;

[GlobalClass]
public partial class World : Node2D {
	private Camera2D _camera;
	private TileMapLayer _tileMap;
	private PlayerController _player;
	
	[Export] public AudioStream Bgm;
	[Export] public PackedScene PauseMenuScene { get; private set; } 
	public GameGlobal GameGlobal { get; private set; }
	public SoundManager SoundManager { get; private set; }

	public void UpdatePlayer(Vector2 position)
	{
		_player.GlobalPosition = position;
	}
	
	public override void _Ready()
	{
		_player = GetNode<PlayerController>("Player");
		_camera = GetNode<Camera2D>("Player/Camera2D");
		_tileMap = GetNode<TileMapLayer>("TileMap/Platform");
		var used = _tileMap.GetUsedRect().Grow(-1);
		Vector2I tileSize = _tileMap.TileSet.TileSize;
		_camera.LimitTop = used.Position.Y * tileSize.Y;
		_camera.LimitRight = used.End.X * tileSize.X;
		_camera.LimitBottom = used.End.Y * tileSize.Y;
		_camera.LimitLeft = used.Position.X * tileSize.X;
		_camera.ResetSmoothing();
		_camera.ForceUpdateScroll();
		GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
		SoundManager = GetNode<SoundManager>("/root/SoundManager");
		GameGlobal.Camera2D = _camera;
		SoundManager.PlayerBgm(Bgm);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			var pauseMenu = PauseMenuScene.Instantiate<PauseMenu>();
			AddChild(pauseMenu);
			GetTree().Root.SetInputAsHandled();
		}

	}
	
}

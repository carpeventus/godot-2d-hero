using Godot;
using System;

public partial class World : Node2D {
	private Camera2D _camera;
	private TileMapLayer _tileMap;
	
	public GameGlobal GameGlobal { get; private set; } 

	
	public override void _Ready() {
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
		GameGlobal.Camera2D = _camera;
		
	}
}

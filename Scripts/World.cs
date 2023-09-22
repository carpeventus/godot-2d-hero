using Godot;
using System;

public partial class World : Node2D {
	private Camera2D _camera;
	private TileMap _tileMap;
	public override void _Ready() {
		_camera = GetNode<Camera2D>("Player/Camera2D");
		_tileMap = GetNode<TileMap>("TileMap");
		var used = _tileMap.GetUsedRect().Grow(-1);
		Vector2I tileSize = _tileMap.TileSet.TileSize;

		_camera.LimitTop = used.Position.Y * tileSize.Y;
		_camera.LimitLeft = used.Position.X * tileSize.Y;
		_camera.LimitRight = used.End.X * tileSize.X;
		_camera.LimitBottom = used.End.Y * tileSize.X;
		_camera.ResetSmoothing();
	}
	
}

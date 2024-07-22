using Godot;

public partial class PlayerStatus : Resource
{
    [Export] public int Health;
    [Export] public float Energy;
    [Export] public Vector2 Position;
}

using Godot;

public partial class Damage : RefCounted {
    public int amount { get; set; }
    public Node2D source { get; set; }
}
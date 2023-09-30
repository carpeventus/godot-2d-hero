using Godot;

public partial class Enemy : CharacterBody2D
{
    public float DefaultGravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity").AsDouble();

    public int Direction;
    public float CurrentGravity;
    
    protected Node2D SpriteWrap;
    public AnimationPlayer AnimationPlayer;
    
    public override void _Ready() {
        base._Ready();
        Direction = -1;
        CurrentGravity = DefaultGravity;
        SpriteWrap = GetNode<Node2D>("SpriteWrap");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public virtual void Flip() {
        SpriteWrap.Scale = new Vector2(Direction, 1);
        Direction *= -1;
        
    }
}

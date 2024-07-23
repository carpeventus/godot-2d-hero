using Godot;
using System;

public partial class SavePoint : Interactable
{
    public GameGlobal GameGlobal { get; private set; }
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        base._Ready();
        GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void Interact()
    {
        base.Interact();
        _animationPlayer.Play("active");
        GameGlobal.SaveGame();
    }
}

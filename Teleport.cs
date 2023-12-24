using Godot;
using System;

public partial class Teleport : Interactable {
    [Export(PropertyHint.File, "*.tscn")] public string ScenePath;
    [Export] public string ToEntryName;
    public GameGlobal GameGlobal { get; private set; }
    public override void _Ready() {
        base._Ready();
        GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
    }

    public override void Interact() {
        base.Interact();
        GameGlobal.ChangeScene(ScenePath, ToEntryName);
    }
}

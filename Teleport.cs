using Godot;
using System;

public partial class Teleport : Interactable {
    [Export(PropertyHint.File, "*.tscn")] public string ScenePath;
    public override void Interact() {
        base.Interact();
        GetTree().ChangeSceneToFile(ScenePath);
    }
}

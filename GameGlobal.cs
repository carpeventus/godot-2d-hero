using Godot;
using System;

public partial class GameGlobal : Node
{
    public Camera2D Camera2D { get; set; }
    public Stat Stat { get; set; }
    
    public override void _Ready() {
        Stat = GetNode<Stat>("Stat");
        GD.Print(Stat.CurrentEnergy);
    }

    public async void ChangeScene(string path, string entryPointName) {
        GetTree().ChangeSceneToFile(path);
        await ToSignal(GetTree(), SceneTree.SignalName.TreeChanged);
        foreach (var node in GetTree().GetNodesInGroup("EntryPoints")) {
            if (node is EntryPoint entry) {
                if (entry.Name == entryPointName) {
                    MovePlayer(entry);
                    return;
                }
            }
        }
    }
    
    public void MovePlayer(EntryPoint entryPoint) {
        if (GetTree().GetFirstNodeInGroup("Player") is PlayerController player) {
            player.MoveToEntrypoint(entryPoint.GlobalPosition);
            Camera2D.ResetSmoothing();
        }
    }
}

using System.Collections.Generic;
using Godot;
using System.Linq;
using Godot.Collections;

public partial class GameGlobal : Node
{
    public Camera2D Camera2D { get; set; }
    public Stat Stat { get; set; }

    public Godot.Collections.Dictionary<string, EnemyAliveState> WorldState =
        new Godot.Collections.Dictionary<string, EnemyAliveState>();
    public ColorRect ColorRect { get; set; }
    
    public override void _Ready() {
        Stat = GetNode<Stat>("Stat");
        ColorRect = GetNode<ColorRect>("SceneFade/ColorRect");
        ColorRect.Color = new Color(ColorRect.Color,0.0f);
    }

    public async void ChangeScene(string path, string entryPointName)
    {
        var oldSceneName = GetTree().CurrentScene.SceneFilePath.GetBaseName();
        WorldState[oldSceneName] = ToDict(GetTree());
        var tween = GetTree().CreateTween();
        tween.TweenProperty(ColorRect, "color:a", 1, 0.2);
        await ToSignal(tween, Tween.SignalName.Finished);
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
            var tween = GetTree().CreateTween();
            tween.TweenProperty(ColorRect, "color:a", 0, 0.2);
            var newSceneName = GetTree().CurrentScene.SceneFilePath.GetBaseName();
            if (WorldState.ContainsKey(newSceneName))
            {
                FromDict(GetTree(), WorldState[newSceneName]);
            }
            
        }
    }

    public EnemyAliveState ToDict(SceneTree sceneTree)
    {
        var state = new EnemyAliveState();
        var list = new List<string>();
        foreach (var node in sceneTree.GetNodesInGroup("Enemy"))
        {
            var path = GetPathTo(node);
            list.Add(path);
        }

        state.enemyAlive = list;
        return state;
    }
    
    public void FromDict(SceneTree sceneTree, EnemyAliveState aliveState)
    {
        foreach (var node in sceneTree.GetNodesInGroup("Enemy"))
        {
            var path = GetPathTo(node);
            var array = aliveState.enemyAlive;
            if (!array.Contains(path))
            {
                node.QueueFree();
            }
        }

    }
}

public partial class EnemyAliveState : GodotObject
{
    public List<string> enemyAlive { get; set; }
}

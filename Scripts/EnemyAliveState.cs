using Godot;
using Godot.Collections;

public partial class EnemyAliveState : Resource
{
    [Export] public Array<string> enemyAlive { get; set; }
}
using Godot;
using Godot.Collections;


public partial class SavedData : Resource
{
    [Export] public Dictionary<string, EnemyAliveState> WorldState;
    [Export] public PlayerStatus PlayerStatus;
}


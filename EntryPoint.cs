using Godot;
using System;

public partial class EntryPoint : Marker2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("EntryPoints");
	}
	
}

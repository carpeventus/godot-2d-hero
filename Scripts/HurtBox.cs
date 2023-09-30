using Godot;
using System;

[GlobalClass]
public partial class HurtBox : Area2D {
    [Signal]
    public delegate void HurtEventHandler(HitBox hitBox);
    
}

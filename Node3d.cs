using Godot;
using System;
using Taterminal;
public partial class Node3d : Node3D
{
  [Export]
  Bus bus {get;set;}
	public override void _Ready() {
    GD.Print("Hello world.");
    GD.Print(bus);
    bus.Log("hellso");
    bus.Warn("hello");
    bus.Error("boo");
    bus.Count("pollo");
	}

}


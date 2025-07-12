using Godot;
using System;
using InkBridge;
public partial class Boat : Node3D {
	[Export]
	public Node3D Foam {get; set;} 
	private Bus bus;
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

namespace InkBridge {
	
}	
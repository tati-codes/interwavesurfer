using Godot;
using System;
using OBus;
public partial class ChoiceScene : Node3D {
	public Bus bus;
	[Export]
	public MeshInstance3D Text {get; set;} 
	[Export]
	public MeshInstance3D Portal {get; set;} 
	[Export]
	public int index { get; set; }
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
	}

}


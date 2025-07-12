using Godot;
using System;
using OBus;

public partial class TestingQuizAreatscn : Node3D {
	private Bus bus;

	[Export]
	public InkWrapper story {get; set;} 
	[Export]
	public Node3D boat {get; set;} 
	
	//TODO move boat by choices
		//TODO programmatically sail into a specific portal
	//TODO generate text via ink choices
	//TODO portal fadeout
	//TODO boat sway
	//TODO a UI frame for the questions to pop up
	//TODO choice selection
		//TODO choice selection feedback
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

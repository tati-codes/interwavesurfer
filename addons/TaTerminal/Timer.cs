#if TOOLS

using Godot;
using System;
[Tool]
public partial class Timer : Godot.Timer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

#endif
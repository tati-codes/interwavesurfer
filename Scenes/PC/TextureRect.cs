using Godot;
using System;

public partial class TextureRect : Godot.TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.Rotation = 90f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

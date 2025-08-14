using Godot;
using System;
using OBus;
using UIEvents;

public partial class ObeliskChoices : Node3D {
	[Export]
	public Godot.Collections.Array<SpotLight3D> Lights { get; set; }
	public override void _Ready()	{
	}
}


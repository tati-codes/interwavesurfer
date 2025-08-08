using Godot;
using System;
using OBus;
using UIEvents;

public partial class ObeliskChoices : Node3D {
	public Bus bus;
	[Export]
	public Godot.Collections.Array<SpotLight3D> Lights { get; set; }
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		// bus.Subscribe<IShowDialogChoices, ChoiceDialogArgs>(consume);
	}
}


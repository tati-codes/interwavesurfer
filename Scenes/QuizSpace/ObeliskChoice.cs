using Godot;
using System;
using InkBridge;
using OBus;
using QuizSpace;

public partial class ObeliskChoice : Node3D {
	public Bus bus;
	[Export]
	public SpotLight3D light {get; set;}

	[Export] public int idx { get; set; } = 0;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<UIItemHighlighted, SelectionIdx>(args => {
			if (args.index == this.idx) {
				bus.Log((args.index == this.idx).ToString());
				light.Show();
			} else {
				light.Hide();
			}
		});
		bus.Subscribe<ChoiceRequired, InkChoices>((args) => {
			if (this.idx == 0) {
				light.Show();
			} else {
				light.Hide();
			}
		});
	}

}


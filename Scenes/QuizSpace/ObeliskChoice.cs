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
	private SubscriptionHolder subscriptions = new();


	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		var uiSub = bus.Subscribe<UIItemHighlighted, SelectionIdx>(args => {
			if (args.index == this.idx) {
				bus.Log((args.index == this.idx).ToString());
				light.Show();
			} else {
				light.Hide();
			}
		});
		var choiceSub = bus.Subscribe<ChoiceRequired, InkChoices>((args) => {
			if (this.idx == 0) {
				light.Show();
			} else {
				light.Hide();
			}
		});
		subscriptions.Add(uiSub, choiceSub);
	}

	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
}


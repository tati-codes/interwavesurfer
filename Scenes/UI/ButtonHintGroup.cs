using Godot;
using System;
using Interactables;
using OBus;
using UIEvents;

public partial class ButtonHintGroup : Control {
	Bus bus;
	[Export]
	public FaceButtonZone primaryZone {get; set;} 
	private SubscriptionHolder subscriptions = new();

	public GlobalState global;

	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		subscriptions.Add(
			bus.Subscribe<UITransitionEv, UITransition>(displayScene)
			);
	}

	public void displayScene(UITransition transition) {
		switch (transition.to) {
			case UIState.CAN_PICK_UP:
				primaryZone.show(PlayerInput.Buttons.A, "Pick Up");
				break;
			case UIState.IDLE:
				if (transition.from == UIState.DIALOG) return;
				primaryZone.disappear();
				break;
			case UIState.IS_HOLDING:
				//FIXME
				primaryZone.show(PlayerInput.Buttons.A, "Fix this");
				break;
			case UIState.CAN_READ:
				if (transition.from == UIState.DIALOG) return;
				else primaryZone.show(PlayerInput.Buttons.A, "Read");
				break;
			case UIState.DIALOG:
				primaryZone.disappear();
				break;
			case UIState.FULLSCREEN_DIALOG:
				break;
			case UIState.CAN_MOVE:
				if (global.InteractionTarget is MoverItem mover) {
					primaryZone.show(PlayerInput.Buttons.A, mover.text);
				} else {
					bus.Error("Global target is not a MoverItem: " + global.InteractionTarget);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(transition.to), nameof(transition.from), null);
		}
	}
}


using Godot;
using System;
using Interactables;
using OBus;
using UIEvents;

public partial class ButtonHintGroup : Control {
	Bus bus;
	[Export]
	public FaceButtonZone primaryZone {get; set;} 
  
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<UITransitionEv, UITransition>(displayScene);
	}

	public void displayScene(UITransition transition) {
		switch (transition.to) {
			case UIState.CAN_PICK_UP:
				primaryZone.show(PlayerInput.Buttons.A, "Pick Up");
				break;
			case UIState.IDLE:
				primaryZone.disappear();
				break;
			case UIState.IS_HOLDING:
				break;
			case UIState.CAN_READ:
				primaryZone.show(PlayerInput.Buttons.A, "Read");
				break;
			case UIState.DIALOG:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(transition.to), nameof(transition.from), null);
		}
	}
}


using Godot;
using System;
using Interactables;
using OBus;
using TatiDebug;
using UIEvents;

public partial class UiRoot : Control {
	public Bus bus;
	[ExportGroup("Internal")]
	[Export]
	public DialogRoot DialogZone {get; set;} 
	[Export]
	public Control ToastZone {get; set;} 
	[Export]
	public Control ButtonGroup {get; set;}

	public tag UITag = new("UI", "727dff");
	
	public UIState currentState = UIState.IDLE;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<LookingAt<PickupableItem>, PickupableItem>((args) => switchTo(UIState.CAN_PICK_UP));
		bus.Subscribe<LookingAt<ReadableItem>, ReadableItem>((args) => switchTo(UIState.CAN_READ));
		bus.Subscribe<StoppedLookingAt, NodeRef>(args => switchTo(UIState.IDLE));
		bus.Subscribe<ReadItem, ReadableItem>(args => switchTo(UIState.DIALOG));
		bus.Subscribe<IShowDialog, DialogText>(args => switchTo(UIState.DIALOG));
		bus.Subscribe<FullscreenIShowDialog, DialogText>(args => switchTo(UIState.FULLSCREEN_DIALOG));
		// bus.Subscribe<>
	}
	public void switchTo(UIState nextState) {
		if (nextState == currentState) return;
		var oldState = currentState;
		currentState = nextState;
		bus.Publish<UITransitionEv, UITransition>(new(oldState, nextState));
		bus.IPub<TatiDebug.Debug,DebugVar>(new("CurrentState", nextState));
		// bus.Publish<Debug, DebugVar>(new("state", currentState));
	}
}




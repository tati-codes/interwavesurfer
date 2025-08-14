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
	private SubscriptionHolder subscriptions = new();


	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		var pickupsub = bus.Subscribe<LookingAt<PickupableItem>, PickupableItem>((args) => switchTo(UIState.CAN_PICK_UP));
		var readsub = bus.Subscribe<LookingAt<ReadableItem>, ReadableItem>((args) => switchTo(UIState.CAN_READ));
		var stopsub = bus.Subscribe<StoppedLookingAt, NodeRef>(args => switchTo(UIState.IDLE));
		var readada = bus.Subscribe<ReadItem, ReadableItem>(args => switchTo(UIState.DIALOG));
		var dialog = bus.Subscribe<IShowDialog, DialogText>(args => switchTo(UIState.DIALOG));
		var full = bus.Subscribe<FullscreenIShowDialog, DialogText>(args => switchTo(UIState.FULLSCREEN_DIALOG));
		// bus.Subscribe<>
		subscriptions.Add(pickupsub, readsub, stopsub, readada, dialog, full);
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




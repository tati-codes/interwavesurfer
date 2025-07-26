using Godot;
using System;
using Interactables;
using static PlayerInput;
public partial class PlayerInputHandler : Node {
	public Bus bus;
	public GlobalState global;
	[Export] 
	public InteractingBeam beam { get; set; }
	[Export]
	public CubeOfHolding holdingZone {get; set;}
	public override void _Ready() {
		global = GetNode<GlobalState>("/root/Global");
		bus = GetNode<Bus>("/root/bus");
	}
	public override void _UnhandledInput(InputEvent @event) {
		switch (EventIsAction(@event)) {
			case Actions.PRIMARY:
				handlePrimary();
				break;
			case Actions.LT:
				handleTrigger(Actions.LT);
				break;
			case Actions.RT:
				handleTrigger(Actions.RT);
				break;
			/** movement is handled by the main Player class **/
			#region Movement
				case Actions.RIGHT:
					break;
				case Actions.LEFT:
					break;
				case Actions.BACK:
					break;
				case Actions.FORWARD:
					break;
			#endregion
			case Actions.DEBUG:
				break;
			case Actions.UNKNOWN:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
	private void handlePrimary() {
		if (global.InteractionTarget == null) return;
		//check that the item is placeable TODO??
		if (global.PlayerIsHoldingItem && global.InteractionTarget is PickupableItem droppable) bus.Publish<DropItem, DropItemArgs>(new(droppable.reference, holdingZone.GlobalPosition));
		else if (global.InteractionTarget is PickupableItem pickupable) bus.Publish<PickupItem, PickupableItem>(pickupable);
		else if (global.InteractionTarget is ReadableItem readable) bus.Publish<ReadItem, ReadableItem>(readable);
		//TODO
		//bus.Publish<DisplayTextFrom, ReadableItem>(new(beam.target));
	}
	void handleTrigger(Actions action) {
		if (global.PlayerIsHoldingItem && global.InteractionTarget is PickupableItem pickedUpItem) {
			switch (action) {
				case Actions.LT:
					bus.Publish<RotateHeldItemByXAxis, PickupableItem>(pickedUpItem);
					break;
				case Actions.RT:
					bus.Publish<RotateHeldItemByYAxis, PickupableItem>(pickedUpItem);
					break;
				default: break;
			}
		}
	}
}


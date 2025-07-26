using Godot;
using Interactables;
public partial class GlobalState : Node {
	public Bus bus;
	public bool PlayerIsHoldingItem { get; private set;  } = false;
	public NodeRef InteractionTarget { get; private set;  } = null;
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<PickupItem, PickupableItem>(args => PlayerIsHoldingItem = true);
		bus.Subscribe<DropItem, DropItemArgs>(args => PlayerIsHoldingItem = false);
		bus.Subscribe<LookingAt<ReadableItem>,ReadableItem>(args => InteractionTarget = args);
		bus.Subscribe<LookingAt<PickupableItem>, PickupableItem>(args => InteractionTarget = args);
		bus.Subscribe<StoppedLookingAt, NodeRef>(args => InteractionTarget = null);
	}
}


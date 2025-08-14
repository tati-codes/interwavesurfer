using Godot;
using System;
using Interactables;
using OBus;

[GlobalClass]
public partial class CubeOfHolding : MeshInstance3D {
	Bus bus;
	private Mesh initialMesh;
	private SubscriptionHolder subscriptions = new();


	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
	public override void _Ready() {
		this.initialMesh = this.Mesh;
		bus = GetNode<Bus>("/root/bus");
		subscriptions.Add(
			bus.Subscribe<CopyMeshToCube, ItemMesh>(args => this.Mesh = args.mesh),
			bus.Subscribe<DropItem, DropItemArgs>(args => this.Mesh = this.initialMesh),
			//TODO copy rotation to actual object
			//TODO save initial rotation
			//TODO rotate interacted object while turned off
			bus.Subscribe<RotateHeldItemByXAxis, PickupableItem>(args => this.RotateX(15)),
			bus.Subscribe<RotateHeldItemByYAxis, PickupableItem>(args => this.RotateY(15))
			);
	}
}
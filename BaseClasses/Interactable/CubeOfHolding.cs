using Godot;
using System;
using Interactables;
[GlobalClass]
public partial class CubeOfHolding : MeshInstance3D {
	Bus bus;
	private Mesh initialMesh;
	public override void _Ready() {
		this.initialMesh = this.Mesh;
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<CopyMeshToCube, ItemMesh>(args => this.Mesh = args.mesh);
		bus.Subscribe<DropItem, DropItemArgs>(args => this.Mesh = this.initialMesh);
		//TODO copy rotation to actual object
		//TODO save initial rotation
		//TODO rotate interacted object while turned off
		bus.Subscribe<RotateHeldItemByXAxis, PickupableItem>(args => this.RotateX(15));
		bus.Subscribe<RotateHeldItemByYAxis, PickupableItem>(args => this.RotateY(15));
	}
}
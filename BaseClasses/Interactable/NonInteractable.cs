using Godot;
using System;
using OBus;
using Interactables;
[GlobalClass]
public partial class NonInteractable : Node {
  [Export]
  public CollisionObject3D NonInteractableItem {get; set;} 
	public override void _Ready()	{
		Bus bus = GetNode<Bus>("/root/bus");
    bus.Publish<RegisterNonInteractable, NodeRef>(new() { reference = NonInteractableItem.GetRid() });
	    //
	}
}


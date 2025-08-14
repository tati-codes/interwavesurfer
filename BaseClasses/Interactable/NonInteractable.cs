using Godot;
using System;
using OBus;
using Interactables;
[GlobalClass]
public partial class NonInteractable : Node {
  [Export]
  public CollisionObject3D NonInteractableItem {get; set;}
  private SubscriptionHolder subscriptions = new();


  public override void _ExitTree() {
	  subscriptions.Dispose();
	  base._ExitTree();
  }
	public override void _Ready()	{
		Bus bus = GetNode<Bus>("/root/bus");
    bus.Publish<RegisterNonInteractable, NodeRef>(new(NonInteractableItem.GetRid()));
	    //
	}
}


using Godot;
using System;
using OBus;
using TatiDebug;
using Interactables;
public partial class InteractingBeam : ShapeCast3D {
	Bus bus;
  [Export]
  bool debug {get; set;}
  [Export]
  MeshInstance3D visibleBeam;
	public override void _Ready() {
    bus = GetNode<Bus>("/root/bus");
    bus.Subscribe<RegisterNonInteractable, NodeRef>(args => AddExceptionRid(args.reference));
    if (!debug) {
      Hide();
      foreach (Node item in GetChildren()) {
        item.QueueFree();
      }
    }
  } 
  public override void _Process(double delta) {
    if (IsColliding()){
      for (int i = 0; i < GetCollisionCount(); i++) {
        GodotObject collider = GetCollider(i);
        bus.Publish<LookingAt, NodeRef>(new() { reference = GetColliderRid(i)});
        dbg("Collision Point", GetCollisionPoint(0));
        dbg("Collider", collider);
      }
    } else {
        dbg("Collision Point", "off");
        dbg("Collider", "off");
    }
  }
  public void dbg(string n, Object o) {
    if (debug) bus.IPub<Debug, DebugVar>(new(n,o)); 
  }
}

namespace Interactables {
  public class LookingAt: TEvent<NodeRef>{};
  public class RegisterNonInteractable: TEvent<NodeRef> {}
  public class NodeRef: Args {
    public Godot.Rid reference {get; init;}
  }
}
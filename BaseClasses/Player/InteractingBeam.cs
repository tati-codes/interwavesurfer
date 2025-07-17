using Godot;
using System;
using OBus;
using TatiDebug;
using Interactables;
using System.Diagnostics;
using Debug = TatiDebug.Debug;
public partial class InteractingBeam : ShapeCast3D {
	Bus bus;
  [Export]
  bool debug {get; set;}
  [Export]
  MeshInstance3D visibleBeam;
  Rid _target = default;
  Rid target {get => _target; set {
    if (_target != value) {
      stoppedLookingAt(_target);
      _target = value;
      lookingAt(value);
    }
  }}
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
  void lookingAt(Rid id) {
    if (id == default) return;
    bus.Publish<LookingAt, NodeRef>(new() { reference = id});
  } 
  void stoppedLookingAt(Rid id) {
    if (id == default) return;
    bus.Publish<StoppedLookingAt, NodeRef>(new() { reference = id});
  } 
  public override void _Process(double delta) {
    if (IsColliding()) target = GetColliderRid(0);
    else if (!IsColliding() && target != default) target = default;
  }
}

namespace Interactables {
  public class LookingAt: TEvent<NodeRef>{};
  public class StoppedLookingAt: TEvent<NodeRef>{};
  public class RegisterNonInteractable: TEvent<NodeRef> {}
  public class NodeRef: Args {
    public Godot.Rid reference {get; init;}
  }
}
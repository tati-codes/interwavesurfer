using System;
using Godot;
using OBus;
using Interactables;
public partial class InteractingBeam : ShapeCast3D {
	Bus bus;
  [Export]
  bool debug {get; set;}
  [Export]
  MeshInstance3D visibleBeam;
  [Export]
  CubeOfHolding holdingZone;
  
  bool PlayerIsHoldingItem = false;
  Rid _target;
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
    if (PlayerIsHoldingItem) return;
    if (IsColliding()) target = GetColliderRid(0);
    else if (!IsColliding() && target != default) target = default;
  }
  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionReleased("primary")) handlePrimary();
  }

  private void handlePrimary() {
    if (target == default) return;
    if (PlayerIsHoldingItem) {
      //FIXME check that the item is placeable
      bus.Publish<DropItem, DropItemArgs>(new(target, holdingZone.GlobalPosition));
      target = default;
      PlayerIsHoldingItem = false;
    } else {
      bus.Publish<PickupItem, NodeRef>(new() { reference = target });
      PlayerIsHoldingItem = true;
    }
  }
}

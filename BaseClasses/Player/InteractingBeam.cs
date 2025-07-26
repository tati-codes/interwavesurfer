using Godot;
using Interactables;
using OBus;
using static PlayerInput;
public partial class InteractingBeam : ShapeCast3D {
	Bus bus;
  public GlobalState global;

  [Export]
  bool debug {get; set;}
  [Export]
  MeshInstance3D visibleBeam;

  Rid _target;
  public Rid target {get => _target; private set {
    if (_target != value) {
      stoppedLookingAt(_target);
      _target = value;
      lookingAt(value);
    }
  }}
  public override void _Ready() {
    bus = GetNode<Bus>("/root/bus");
    global = GetNode<GlobalState>("/root/Global");
    bus.Subscribe<RegisterNonInteractable, NodeRef>(args => AddExceptionRid(args.reference));
    bus.Subscribe<DropItem, DropItemArgs>(args => {
      if (target == args.reference) target = default;
    });
    if (!debug) Hide();
  }
  void lookingAt(Rid id) {
    if (id == default) return;
    bus.Publish<LookingAt<NodeRef>, NodeRef>(new(id));
  } 
  void stoppedLookingAt(Rid id) {
    if (id == default) return;
    bus.Publish<StoppedLookingAt, NodeRef>(new(id));
  } 
  public override void _Process(double delta) {
    if (global.PlayerIsHoldingItem) return;
    if (IsColliding()) target = GetColliderRid(0);
    else if (!IsColliding() && target != default) target = default;
  }

}

namespace Interactables {
  public class LookingAt<T>: TEvent<T> where T: NodeRef { };
  public class StoppedLookingAt: TEvent<NodeRef> { };
  public class RegisterNonInteractable: TEvent<NodeRef> { }
}
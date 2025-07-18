using Godot;
using System;
using OBus;
using Interactables;
[GlobalClass]
public partial class Interactable : Node {
	public Bus bus;
  [Export]
  public CollisionObject3D collider {get; set;} 
  [Export]
  public GeometryInstance3D overlaySetter {get; set;} //there's a geometry3dinstance and geometryinstance3d

  [Export] public MeshInstance3D meshHolder { get; set; }
  [Export]
  public Material black {get; set;} 
  [Export]
  public Material white {get; set;} 
	public override void _Ready() {
    bus = GetNode<Bus>("/root/bus");
    bus.Subscribe<StoppedLookingAt, NodeRef>(args => refersToMe(args.reference, unhandleGaze));
    bus.Subscribe<LookingAt, NodeRef>(args => refersToMe(args.reference, handleGaze));
    bus.Subscribe<PickupItem, NodeRef>(args => refersToMe(args.reference, handlePickup));
    bus.Subscribe<DropItem, DropItemArgs>(args => refersToMe(args.reference, () => handleDrop(args.position)));
    overlaySetter.MaterialOverlay = black;
	}
  void handleDrop(Vector3 pos) {
    meshHolder.Position = pos;
    meshHolder.Show();
  }
  void handlePickup() {
    bus.Publish<CopyMeshToCube, ItemMesh>(new(meshHolder.Mesh));
    meshHolder.Hide();
  }
  void refersToMe(Rid rid, Action callback) {
    if (rid == collider.GetRid()) {
      callback();
    }
  }
  void handleGaze() => overlaySetter.MaterialOverlay = white;
  void unhandleGaze() => overlaySetter.MaterialOverlay = black;
}

using Godot;
using System;
using OBus;
using Interactables;
[GlobalClass]
public partial class Interactable : Node {
	Bus bus;
  [Export]
  public CollisionObject3D collider {get; set;} 
  [Export]
  public GeometryInstance3D overlaySetter {get; set;} //there's a geometry3dinstance and geometryinstance3d
  [Export] public MeshInstance3D meshHolder { get; set; }
  public Material black;
  public Material white;
	public override void _Ready() {
    white = GD.Load<Material>("res://Assets/Shaders/White.tres");
    black = GD.Load<Material>("res://Assets/Shaders/BlackOutlinerMaterial.tres");
    bus = GetNode<Bus>("/root/bus");
    bus.Subscribe<StoppedLookingAt, NodeRef>(args => refersToMe(args.reference, unhandleGaze));
    bus.Subscribe<LookingAt<NodeRef>, NodeRef>(args => refersToMe(args.reference, handleGaze));
    overlaySetter.MaterialOverlay = black;
	}
  protected void refersToMe(Rid rid, Action callback) {
	  if (rid == collider.GetRid()) callback();
  }
  protected bool refersToMe(Rid rid) => rid == collider.GetRid();
  void handleGaze() => overlaySetter.MaterialOverlay = white;
  void unhandleGaze() => overlaySetter.MaterialOverlay = black;
}

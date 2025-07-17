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
  public GeometryInstance3D mesh {get; set;} //there's a geometry3dinstance and geometryinstance3d
  public Material black = GD.Load<Material>("res://BaseClasses/Interactable/BlackOutlinerMaterial.tres");
  public Material combined = GD.Load<Material>("res://BaseClasses/Interactable/Combined.tres");

	public override void _Ready() {
    Bus bus = GetNode<Bus>("/root/bus");
    bus.Subscribe<StoppedLookingAt, NodeRef>(args => unhandleGaze(args.reference));
    bus.Subscribe<LookingAt, NodeRef>(args => handleGaze(args.reference));
    mesh.MaterialOverlay = black;
	}
  bool refersToMe(Rid rid) => rid == collider.GetRid(); 
  void handleGaze(Rid rid) {
    if (!refersToMe(rid)) return; 
    mesh.MaterialOverlay = combined;
  }
  void unhandleGaze(Rid rid) {
    if (!refersToMe(rid)) return; 
    mesh.MaterialOverlay = black;
  }
}

namespace Interactables {

}
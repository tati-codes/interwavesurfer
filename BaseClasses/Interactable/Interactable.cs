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
  public MeshInstance3D mesh {get; set;}
  [Export]
  public Material highlighter {get; set;} 
	public override void _Ready() {
    Bus bus = GetNode<Bus>("/root/bus");
    bus.Subscribe<LookingAt, NodeRef>(args => handleGaze(args.reference));
    bus.Subscribe<StoppedLookingAt, NodeRef>(args => unhandleGaze(args.reference));
	}
  bool refersToMe(Rid rid) => rid == collider.GetRid(); 
  void handleGaze(Rid rid) {
    if (!refersToMe(rid)) return; 
    mesh.MaterialOverlay = highlighter;
  }
  void unhandleGaze(Rid rid) {
    if (!refersToMe(rid)) return; 
    mesh.MaterialOverlay = null;
  }
}

namespace Interactables {

}
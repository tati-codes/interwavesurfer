using Godot;
using Interactables;
using OBus;
using State;

[GlobalClass]
public partial class Mover : Interactable {
  Bus bus;
  [Export]
  public sceneEnum scene {get; set;} 
  [Export]
  public string text {get; set;}
  
  private SubscriptionHolder subscriptions = new();
  public override void _ExitTree() {
    subscriptions.Dispose();
    base._ExitTree();
  }
  public override void _Ready() {
    base._Ready();
    bus = GetNode<Bus>("/root/bus");
    subscriptions.Add(
    bus.Subscribe<LookingAt<NodeRef>, NodeRef>(args => 
      refersToMe(args.reference, () => 
        bus.Publish<LookingAt<MoverItem>, MoverItem>(new MoverItem(collider.GetRid(), scene, text))))
    );
  }
}

namespace Interactables {
  public class MoverItem(Godot.Rid reference, sceneEnum scene, string text) : NodeRef(reference) {
    public sceneEnum nextScene { get; init; } = scene;
    public string text { get; init; } = text;
  }
  public class MoveInto: TEvent<MoverItem> { }
}
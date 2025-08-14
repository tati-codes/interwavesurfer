using Godot;
using Interactables;
using OBus;

[GlobalClass]
public partial class Readable : Interactable {
   Bus bus;
  public ReadableItem contextRef => new(collider.GetRid(), object_id, interactionTag);
  [Export]
  public string object_id {get; set;}

  [Export] public string interactionTag { get; set; } = "Read";
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
        bus.Publish<LookingAt<ReadableItem>, ReadableItem>(contextRef)))
    );
  }
}

namespace Interactables {
  public class ReadableItem(Godot.Rid reference, string text_id, string _tag) : NodeRef(reference) {
    public string text_id { get; init; } = text_id;
    public string inter_tag { get; init; } = _tag;
  }
  public class ReadItem: TEvent<ReadableItem> { }
}
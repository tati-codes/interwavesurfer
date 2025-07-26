using Godot;
using Interactables;
using OBus;

[GlobalClass]
public partial class Readable : Interactable {
   Bus bus;
  public ReadableItem contextRef => new(collider.GetRid(), object_id);
  [Export]
  public string object_id {get; set;} 
  public override void _Ready() {
    base._Ready();
    bus = GetNode<Bus>("/root/bus");
    
    bus.Subscribe<LookingAt<NodeRef>, NodeRef>(args => 
      refersToMe(args.reference, () => 
        bus.Publish<LookingAt<ReadableItem>, ReadableItem>(contextRef)));
  }
}

namespace Interactables {
  public class ReadableItem(Godot.Rid reference, string text_id) : NodeRef(reference) {
    public string text_id { get; init; } = text_id;
  }
  public class ReadItem: TEvent<ReadableItem> { }
}
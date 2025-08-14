using Godot;
using Interactables;
using OBus;

[GlobalClass]
public partial class Pickupable : Interactable {
  Bus bus;
  public PickupableItem contextRef => new(collider.GetRid());
  public string interactionTag { get; set; } = "Pick Up";
  private SubscriptionHolder subscriptions = new();


  public override void _ExitTree() {
    subscriptions.Dispose();
    base._ExitTree();
  }
  public override void _Ready() {
    base._Ready();
    bus = GetNode<Bus>("/root/bus");
    subscriptions.Add(
      bus.Subscribe<PickupItem, PickupableItem>(args => refersToMe(args.reference, handlePickup)),
      bus.Subscribe<DropItem, DropItemArgs>(args => refersToMe(args.reference, () => handleDrop(args.position))),
      /* Not the prettiest code over here but we gotta rebroadcast the general events into more specific ones*/
      bus.Subscribe<LookingAt<NodeRef>, NodeRef>(args => 
        refersToMe(args.reference, () => 
          bus.Publish<LookingAt<PickupableItem>, PickupableItem>(contextRef)))
    );
  }
  void handleDrop(Vector3 pos) {
    meshHolder.Position = pos;
    meshHolder.Show();
  }
  void handlePickup() {
    bus.Publish<CopyMeshToCube, ItemMesh>(new(meshHolder.Mesh));
    meshHolder.Hide();
  }
}

namespace Interactables {
  public class PickupableItem(Godot.Rid reference) : NodeRef(reference) { }
  public class PickupItem: TEvent<PickupableItem> { } 
  /// <summary>
  /// This just returns the item to its original location
  /// </summary>
  public class UndoPickupItem: TEvent<NodeRef> { }

}
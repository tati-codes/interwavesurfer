using Godot;
using OBus;

namespace Interactables {
  //TODO create an UI node that pops up when LookingAt something
  public class LookingAt: TEvent<NodeRef>{};
  public class StoppedLookingAt: TEvent<NodeRef>{};
  public class RegisterNonInteractable: TEvent<NodeRef> {}
  public class NodeRef: Args {
    public Godot.Rid reference {get; init;}
  }
  
  //WHO fires this?
    //Interacting Beam
  //Who responds to it?
  public class PickupItem: TEvent<NodeRef> {} 
  /// <summary>
  /// This just returns the item to its original location
  /// </summary>
  public class UndoPickupItem: TEvent<NodeRef> {}
  //TODO public class DropItem
  public class CopyMeshToCube: TEvent<ItemMesh> {}
  public class DropItem: TEvent<DropItemArgs> {}
  public class DropItemArgs(Godot.Rid reference, Godot.Vector3 position) : Args {
    public Godot.Rid reference { get; init; } = reference;
    public Godot.Vector3 position { get; init; } = position;
  }
	public class ItemMesh(Mesh mesh) : Args {
      public Mesh mesh { get; init; } = mesh;
    }
}
using Godot;
using OBus;

namespace Interactables {
  public class NodeRef(Godot.Rid reference): Args {
    public Godot.Rid reference { get; init; } = reference;
  }
  public class DropItem: TEvent<DropItemArgs> { }
  public class DropItemArgs(Godot.Rid reference, Godot.Vector3 position) : Args {
    public Godot.Rid reference { get; init; } = reference;
    public Godot.Vector3 position { get; init; } = position;
  }
  //TODO move this to holdingcube
  public class CopyMeshToCube: TEvent<ItemMesh> { }
	public class ItemMesh(Mesh mesh) : Args {
      public Mesh mesh { get; init; } = mesh;
    }
  //TODO end of region
  public class RotateHeldItemByXAxis : TEvent<PickupableItem> { }
  public class RotateHeldItemByYAxis : TEvent<PickupableItem> { }
}
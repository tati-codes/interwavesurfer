#if TOOLS

using Godot;
using System;
using Taterminal;

[Tool][GlobalClass]
public partial class ButtonCreator : VBoxContainer {
	[Export]
	public TagHolder tagHolder { get; set; }
	[Export]
	public PackedScene tagButtonScene { get; set; }

	public void rebuild(NArgs _ = default) {
		var children = GetChildren();
		foreach (Node child in children) {
			TagButton temp = child as TagButton;
			if (temp != null) {
        RemoveChild(child);
				child.QueueFree();
			}
		}
    foreach ((tag _tag, var tag_show) in tagHolder.registeredTags) {
	    if (_tag == tag.count && tagHolder.beenCounting == false) {
		    continue;
	    };
      var p = tagButtonScene.Instantiate<TagButton>();
      p.setTag(_tag);
      p.tagHolder = tagHolder;
      p.SetPressedNoSignal(!tag_show);
      AddChild(p);
    }
	}
}

namespace Taterminal
{
    public class RebuildTagButtons : TEvent<NArgs> { }

}
#endif 

#if TOOLS

using Godot;
using System;
using Taterminal;

[Tool]
public partial class ButtonCreator : VBoxContainer {
	[Export]
	public TagHolder tagHolder { get; set; }
	[Export]
	public PackedScene pathToThingButton { get; set; }
	public override void _Ready() {
		// rebuild();
	}
	public void rebuild(NArgs _ = default) {
		// var children = GetChildren();
		// children.RemoveAt(0);
		// foreach (Node child in children) {
		// 	TagButton temp = child as TagButton;
		// 	if (temp != null) {
    //     RemoveChild(child);
		// 		child.QueueFree();
		// 	}
		// }
    // foreach ((tag _tag, var tag_show) in tagHolder.registeredTags) {
    //   var p = pathToThingButton.Instantiate<TagButton>();
    //   p.Tag = _tag; 
    //   p.Text = _tag.name;
    //   p.SetPressedNoSignal(!tag_show);
    //   var new_stylebox_normal = p.GetThemeStylebox("normal").Duplicate();
    //   StyleBoxFlat style = (StyleBoxFlat)new_stylebox_normal;
    //   style.BorderColor = new Color(_tag.bbcolor);
    //   p.AddThemeStyleboxOverride("normal", style);
    //   p.tagHolder = tagHolder;
    //   AddChild(p);
    // }
	}
}

namespace Taterminal
{
    public class RebuildTagButtons : TEvent<NArgs> { }

}
#endif 

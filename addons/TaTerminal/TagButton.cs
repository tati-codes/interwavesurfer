#if TOOLS
using Godot;
using System;
using Taterminal;
[Tool]
public partial class TagButton : Button {
    public tag Tag { get; set; } = tag.nullTag;
    [Export]
    public TagHolder tagHolder;
    public void setTag(tag tag) {
        Tag = tag;
    }
    public override void _Toggled(bool hide) {
        if (this.Tag != tag.nullTag) {
            tagHolder.toggleTag(this.Tag);
        }
    }
}
#endif
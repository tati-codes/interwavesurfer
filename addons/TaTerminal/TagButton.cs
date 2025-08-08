#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using Taterminal;
[Tool]
public partial class TagButton : Button {
    [Export]
    public string tagName {  get; set; }
    [Export]
    public TagHolder tagHolder;
    public void setTag(tag @tag) {
        this.Text = @tag.name;
        this.tagName = @tag.name;
        var new_stylebox_normal = GetThemeStylebox("normal").Duplicate();
        StyleBoxFlat style = (StyleBoxFlat)new_stylebox_normal;
        style.BorderColor = new Color(@tag.bbcolor);
        AddThemeStyleboxOverride("normal", style);
    }
    public override void _Toggled(bool hide) => tagHolder.toggleTag(tagName);
}
#endif
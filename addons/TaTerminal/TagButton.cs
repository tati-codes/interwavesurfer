#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using Taterminal;
[Tool]
public partial class TagButton : Button {

    enum tags {
      info,
      warn,
      error,
      evnt,
      count
    }

    Dictionary<tags, tag> lookup = new() {
      [tags.info] = tag.info,
      [tags.warn] = tag.warning,
      [tags.error] = tag.error,
      [tags.evnt] = tag.evnt,
      [tags.count] = tag.count
    };
    tag actual_tag = tag.info; 
    tags display_tag = tags.info;
    [Export]
    tags Tag { get => display_tag; set {
      display_tag = value; 
    } }

    [Export]
    public TagHolder tagHolder;

    public override void _Ready()
    {
      setTag(display_tag);
    }

    void setTag(tags @tag) {
        display_tag = @tag;
        actual_tag = lookup[@tag];
        this.Text = actual_tag.name;
        var new_stylebox_normal = GetThemeStylebox("normal").Duplicate();
        StyleBoxFlat style = (StyleBoxFlat)new_stylebox_normal;
        style.BorderColor = new Color(actual_tag.bbcolor);
        AddThemeStyleboxOverride("normal", style);
    }
    public override void _Toggled(bool hide) {
        if (actual_tag != tag.nullTag) {
            tagHolder.toggleTag(actual_tag);
        }
    }
}
#endif
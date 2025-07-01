#if TOOLS

using Godot;
using Taterminal;
using System;

[Tool]
public partial class Output : RichTextLabel
{
    public override void _Ready()
    {
        this.VerticalAlignment = VerticalAlignment.Bottom; 
    }
    public void Display(string rs)
    {
      this.Clear();
      this.ParseBbcode(rs);
    }

}

#endif
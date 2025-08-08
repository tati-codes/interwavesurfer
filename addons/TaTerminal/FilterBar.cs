using Godot;
using System;
using Taterminal;
[Tool]
public partial class FilterBar : LineEdit
{
  [Export]
  public Buffer buffer;
	public override void _Ready() {
    this.TextChanged += buffer.setFilter;
	}
}
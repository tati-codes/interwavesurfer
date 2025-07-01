using Godot;
using System;
using Taterminal;

public partial class FilterBar : LineEdit
{
  [Export]
  public Buffer buffer;
	public override void _Ready() {
    this.TextChanged += buffer.setFilter;
	}

}

namespace Taterminal {
  public class FilterStringChanged : TEvent<Text> {}
}
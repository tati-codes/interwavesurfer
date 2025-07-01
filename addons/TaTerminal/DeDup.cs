using Godot;
using System;
using Taterminal;

[Tool][GlobalClass]
public partial class DeDup : Button
{
  [Export]
  public Buffer buffer;
  public override void _Pressed() => buffer.toggleGlobalCollapse();
}

namespace Taterminal {
  public class ToggleGlobalCollapse: TEvent<NArgs> {}
}

#if TOOLS
using Godot;
using Taterminal;
 
[Tool]
public partial class ClearBtn : Button
{
    [Export]
    public Buffer buffer;
    public override void _Pressed() => buffer.Clear();
}
#endif

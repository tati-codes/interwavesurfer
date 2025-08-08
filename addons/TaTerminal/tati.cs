#if TOOLS
using Godot;
using System;

[Tool]
public partial class tati : EditorPlugin
{
    private Control _dock;
    public override void _EnterTree()
    {
        _dock = GD.Load<PackedScene>("res://addons/TaTerminal/UI.tscn").Instantiate<Control>();
        AddControlToBottomPanel(_dock, "TaTerminal");
    }

    public override void _EnablePlugin()
    {
        base._EnablePlugin();
    }
    public override void _DisablePlugin()
    {
        base._DisablePlugin();
    }

    public override void _ExitTree()
    {
        RemoveControlFromBottomPanel(_dock);
        _dock.Free();
    }
}
#endif

#if TOOLS
using Godot;
using System;

[Tool]
public partial class TatiOBusHTTP : EditorPlugin  {
    public override void _EnablePlugin() => AddAutoloadSingleton("bus", "res://addons/OBus/Bus.cs");

    public override void _DisablePlugin() => RemoveAutoloadSingleton("bus");
}
#endif

#if TOOLS

using Godot;
using System;

[Tool]
public partial class OptionButton : Godot.OptionButton
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public Buffer holder { get; set; }
	public override void _Ready()
	{
    this.ItemSelected += OptionButton_ItemSelected;
	}
    private void OptionButton_ItemSelected(long index) => holder.setOverride((Taterminal.LOG_LEVEL)index);
    // Called every frame. 'delta' is the elapsed time since the previous frame.

}
#endif
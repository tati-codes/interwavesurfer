using Godot;
using System;
using OBus;
public partial class ChoiceDialogTextHolder : ScrollContainer {
	[Export]
	public VBoxContainer Container {get; set;} 
	public override void _Ready()	{
		
	}

	public void Append(string text) {
		Label newLabel = new Label();
		newLabel.Text = text;	
		Container.AddChild(newLabel);
	}
}

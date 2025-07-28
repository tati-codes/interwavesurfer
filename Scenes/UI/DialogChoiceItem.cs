using Godot;
using System;
using OBus;
public partial class DialogChoiceItem : Control {
	public Bus bus;
	[Export]
	public TextureRect Arrow {get; set;} 
	[Export]
	public Label ChoiceText {get; set;}

	private bool isSelected = false;
	public void setChoiceText(string text) {
		ChoiceText.Text = text;
	}
	public void Select() => Arrow.SetInstanceShaderParameter("Bouncing", true);
	public void Deselect() => Arrow.SetInstanceShaderParameter("Bouncing", false);


}


using Godot;
using System;
using OBus;
public partial class ButtonZone : Control {
	[Export]
	public ButtonHint image {get; set;} 
	[Export]
	public Label label {get; set;} 
	public void show(PlayerInput.Buttons button, string text) {
		image.setButton(button);
		label.Text = text;;
	}

	public void show(PlayerInput.Buttons button, string text, int scale) {
		image.setButton(button, scale);
		label.Text = text;
	}

	public void reset() {
		image.stopShowing();
		label.Text = "";
	}
}


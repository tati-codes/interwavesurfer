using Godot;
using System;
using OBus;
public partial class FaceButtonIcon : Control {
	public Bus bus;
	[Export]
	public TextureRect icon {get; set;} 
	[Export]
	public Label label {get; set;} 
	 
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		this.SelfModulate = new Godot.Color();
	}

}

namespace UIUtils {
	public enum face_buttons {
		A,
		B,
		X,
		Y
	}
	public struct ButtonConfig {
		public face_buttons button;
		public Color color;

		public static ButtonConfig A = new() {
			button = face_buttons.A,
			// color = 
		};
	}
}

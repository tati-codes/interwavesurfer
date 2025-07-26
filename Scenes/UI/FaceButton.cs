using Godot;
using System;

[GlobalClass]
public partial class FaceButton : TextureRect {
	[Export]
	public Texture2D A {get; set;} 
	[Export]
	public Texture2D B {get; set;} 
	[Export]
	public Texture2D X {get; set;} 
	[Export]
	public Texture2D Y {get; set;}

	public void setButton(PlayerInput.Buttons? button) {
		if (button == null) this.Texture = null;
		else switch (button) {
			case PlayerInput.Buttons.A:
				this.Texture = A;
				break;
			case PlayerInput.Buttons.B:
				this.Texture = B;
				break;
			case PlayerInput.Buttons.X:
				this.Texture = X;
				break;
			case PlayerInput.Buttons.Y:
				this.Texture = Y;
				break;
			default:
				break;
		}
	}
}

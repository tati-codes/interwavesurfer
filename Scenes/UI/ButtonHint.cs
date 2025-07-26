using Godot;
using System;
using System.Collections.Generic;
using Godot.NativeInterop;

[GlobalClass]
public partial class ButtonHint : TextureRect {
	public Bus bus;
	
	[Export]
	public PlayerInput.Buttons currentButton = PlayerInput.Buttons.A;
	[ExportGroup("Textures")]
	[Export]
	public Texture2D a_button {get; set;} 
	[Export]
	public Texture2D x_button {get; set;} 
	[Export]
	public Texture2D y_button {get; set;} 
	[Export]
	public Texture2D b_button {get; set;} 
	[Export]
	public Texture2D dpad_right {get; set;} 
	[Export]
	public Texture2D dpad_left {get; set;} 
	[Export]
	public Texture2D dpad_up {get; set;} 
	[Export]
	public Texture2D dpad_down {get; set;} 
	[Export]
	public Texture2D joystick_left {get; set;} 
	[Export]
	public Texture2D joystick_right {get; set;} 
	[Export]
	public Texture2D joystick_up {get; set;}
	[Export]
	public Texture2D joystick_down {get; set;} 
	[Export]
	public Texture2D l_trigger {get; set;}
	[Export]
	public Texture2D r_trigger {get; set;}
  [Export]
  public Texture2D triangle	{get; set;}

  private Dictionary<PlayerInput.Buttons, Texture2D> button_texture;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		button_texture= new () {
			[PlayerInput.Buttons.A] = a_button,
			[PlayerInput.Buttons.B] = b_button,
			[PlayerInput.Buttons.X] = x_button,
			[PlayerInput.Buttons.Y] = y_button,
			[PlayerInput.Buttons.DPAD_DOWN] = dpad_down,
			[PlayerInput.Buttons.DPAD_UP] = dpad_up,
			[PlayerInput.Buttons.DPAD_LEFT] = dpad_left,
			[PlayerInput.Buttons.DPAD_RIGHT] = dpad_right,
			[PlayerInput.Buttons.JOY_DOWN] = joystick_down,
			[PlayerInput.Buttons.JOY_UP] = joystick_up,
			[PlayerInput.Buttons.JOY_LEFT] = joystick_left,
			[PlayerInput.Buttons.JOY_RIGHT] = joystick_right,
			[PlayerInput.Buttons.LB] = l_trigger,
			[PlayerInput.Buttons.RB] = r_trigger,
			[PlayerInput.Buttons.TRIANGLE] = triangle,
		};
		this.Texture = button_texture[currentButton];
	}
	public void setButton(PlayerInput.Buttons button) {
		this.Texture = button_texture[button];
	}
	public void setButton(PlayerInput.Buttons button, int scale) {
		setButton(button);
		this.Scale = new Vector2(scale, scale);
	}

	public void stopShowing() {
		this.Texture = null;
	}
}

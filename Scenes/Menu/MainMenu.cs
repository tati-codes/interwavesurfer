using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodotInk;
using OBus;
using State;

public partial class MainMenu : Node3D {
	[Export]
	public FaceButtonZone buttons {get; set;} 
	 
	[Export]
	public DialogWithChoicesRoot dialog {get; set;} 
	[Export]
	public AnimationPlayer anim {get; set;} 
	bool dialog_visible = false;
	public Bus bus;
	public override async void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		await Task.Delay(1500);
		buttons.show(PlayerInput.Buttons.A, "Continue");
		dialog.ContentContainer.Maximize();
	}
	public override void _Input(InputEvent @event) {
		if (@event.IsActionReleased("primary")) handlePrimary();
	}
	void handlePrimary() {
		if (!dialog_visible) {
			anim.Play("ShowDialog");
			dialog.Show();
			anim.AnimationFinished += (anim) => {
				if (anim ==  "ShowDialog") {
					dialog.force(new("How can you continue before you start?", new List<InkChoice>(), "???"));
				}
				dialog_visible = true;
			};
		} else {
			bus.Publish<GoToScene, SceneArgs>(new(sceneEnum.QUIZ));
		}
	}
	//LISTEN FOR PRIMARY
	//"How can you continue before you start?"
	//SWITCH TO QUIZ
	//END OF QUIZ, SWITCH OVERWORLD.

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

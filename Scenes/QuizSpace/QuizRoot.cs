using Godot;
using System;
using InkBridge;
using OBus;
using QuizSpace;
using TatiDebug;

public partial class QuizRoot : Node3D {
	private Bus bus;
	[Export]
	public InkWrapper story {get; set;} 
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		// bus.Subscribe<ChoiceSelected>(playerChoiceReceived);
		if (story.currentText == string.Empty) {
			story.Continue();
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionReleased("primary")) handlePrimary();
		else if (@event.IsActionReleased("move_left") || @event.IsActionReleased("move_right")) handleDirections(@event);
	}
	void handlePrimary() {
	}
	void handleDirections(InputEvent @event) {
	}
	void handleUISelect(bool next) {
	}
	public void transitionTo(QuizState nextState) {
	}
}

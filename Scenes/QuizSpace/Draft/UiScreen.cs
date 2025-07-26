using Godot;
using System;
using InkBridge;
using QuizSpace;

public partial class UiScreen : MeshInstance3D {
	private Bus bus;
	[Export]
	public RichTextLabel QuestionText {get; set;} 
	[Export]
	public HBoxContainer textContainer {get; set;}
	
  
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<StoryUpdated, StoryText>(args => QuestionText.Text = args.Text);
		bus.Subscribe<QuizStateTransition, QuizStateTransitionArgs>(handleStateTransition);
	}

	public void handleStateTransition(QuizStateTransitionArgs args) {
		if (args.from == QuizState.SHOW_QUIZ_QUESTION && args.to == QuizState.AWAIT_PLAYER_CHOICE) {
			this.Hide();
			textContainer.Hide();
		} else if (args.from == QuizState.AWAIT_PLAYER_CHOICE && args.to == QuizState.SHOW_QUIZ_QUESTION) {
			this.Show();
			textContainer.Show();
		}
	}
}

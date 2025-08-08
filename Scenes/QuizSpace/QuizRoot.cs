using Godot;
using System;
using InkBridge;
using OBus;
using QuizSpace;
using TatiDebug;
using UIEvents;

public partial class QuizRoot : Node3D {
	private GlobalState global;
	private Bus bus;
	[Export]
	public InkWrapper story {get; set;} 
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		// bus.Subscribe<ChoiceSelected>(playerChoiceReceived);
		bus.Subscribe<StoryUpdated, StoryText>(args => {
			bus.Publish<IShowDialogChoices, ChoiceDialogArgs>(new(args.Text, new(), "???"));
		});
		bus.Subscribe<ChoiceRequired, InkChoices>(args => 
			bus.Publish<IShowDialogChoices, ChoiceDialogArgs>(new(args.line, args.choices, "???"))
		);
		
	}
	
	public override void _Input(InputEvent @event) {
		if (@event.IsActionReleased("primary")) handlePrimary();
		else if (@event.IsActionReleased("move_left") || @event.IsActionReleased("move_right")) handleDirections(@event);
	}
	void handlePrimary() {
		if (story.canContinue) story.Continue();
		else bus.Publish<SelectChoice, IChoice>(new IChoice(global.QuizState.selectedChoice));
	}
	void handleDirections(InputEvent @event) {
		bool eventIsRight = @event.IsActionReleased("move_right");
		bool eventIsLeft = @event.IsActionReleased("move_left");
		if (global.QuizState.currentChoices.Count <= 0) return;
		if (eventIsLeft) handleUISelect(next: false);
		else if (eventIsRight) handleUISelect(next: true);
	}
	void handleUISelect(bool next) {
		if (global.QuizState.selectedChoice != null &&
		    global.QuizState.currentChoices.Count > 1) {
			int UISelectedIndex = global.QuizState.selectedChoice.Index; 
			int UIMaxIdx = global.QuizState.currentChoices.Count;
			if (!next) { //we do it this way because visually the array is laid as 021. dont worry about it
				UISelectedIndex++;
				if (UISelectedIndex >= UIMaxIdx) UISelectedIndex = 0;
			} else {
				UISelectedIndex--;
				if (UISelectedIndex < 0) UISelectedIndex = UIMaxIdx - 1;
			}
			bus.Publish<UIItemHighlighted, SelectionIdx>(new(UISelectedIndex));
		}
	}
}

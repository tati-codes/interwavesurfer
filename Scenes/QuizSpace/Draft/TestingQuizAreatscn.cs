using Godot;
using System;
using InkBridge;
using OBus;
using QuizSpace;
using TatiDebug;

public partial class TestingQuizAreatscn : Node3D {
	private Bus bus;

	[Export]
	public InkWrapper story {get; set;} 
	[Export]
	public Node3D boat {get; set;}
	public QuizState currentState = QuizState.START;
	int UIMaxIdx => story.CurrentChoices.Count - 1;
	int UISelectedIndex = -1;
	private bool animation_lock = false;

	//TODO portal fadeout
	//TODO? boat sway
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Publish<Debug, DebugVar>(new("state", currentState));
		bus.Subscribe<ChoiceSelected>(playerChoiceReceived);
		bus.Subscribe<BoatReachedPortal>(args => {
				bus.Log("CurrentSelection: ", UISelectedIndex.ToString());
				bus.Publish<SelectChoice, IChoice>(new(story.CurrentChoices[UISelectedIndex]));
		});
		if (story.currentText == string.Empty) {
			story.Continue();
		}
		transitionTo(QuizState.SHOW_QUIZ_QUESTION);
	}

	void playerChoiceReceived(NArgs args) {
		animation_lock = false;
		UISelectedIndex = -1;
		if (!story.canContinue) {
			bus.Error("player choice received; can't continue");
			foreach (var storyCurrentChoice in story.CurrentChoices) {
				bus.Inspect(storyCurrentChoice);
			}
		} else {
			story.Continue();
		}
		transitionTo(QuizState.SHOW_QUIZ_QUESTION);		
	}
	public override void _Input(InputEvent @event) {
		if (@event.IsActionReleased("primary")) handlePrimary();
		else if (@event.IsActionReleased("move_left") || @event.IsActionReleased("move_right")) handleDirections(@event);
	}
	void handlePrimary() {
		if (animation_lock) return;
		switch (currentState) {
			case QuizState.SHOW_QUIZ_QUESTION: {
				if (story.canContinue) story.Continue();
				else transitionTo(QuizState.AWAIT_PLAYER_CHOICE);
				break;
			}
			case QuizState.AWAIT_PLAYER_CHOICE: {
				if (UISelectedIndex != -1) {
					animation_lock = true;
					bus.Publish<UIChoiceSelected, SelectionIdx>(new(UISelectedIndex));
				}
				break;
			}
			case QuizState.START: {
				break;
			}
		}
	}
	void handleDirections(InputEvent @event) {
		if (animation_lock) return;
		bool eventIsRight = @event.IsActionReleased("move_right");
		bool eventIsLeft = @event.IsActionReleased("move_left");
		if (eventIsLeft == false && eventIsRight == false) {
			bus.Error("handleDirections is getting a weird event", @event.ToString());
			return;
		}
		switch (currentState) {
			case QuizState.START: break;
			case QuizState.SHOW_QUIZ_QUESTION: break;
			case QuizState.AWAIT_PLAYER_CHOICE: {
				if (eventIsLeft) handleUISelect(next: false);
				else if (eventIsRight) handleUISelect(next: true);
				break;
			}
		}
	}
	void handleUISelect(bool next) {
		if (animation_lock) return;
		if (UISelectedIndex == -1) {
			UISelectedIndex = 0;
		} else if (!next) { //we do it this way because visually the array is laid as 021. dont worry about it
			UISelectedIndex++;
			if (UISelectedIndex > UIMaxIdx) UISelectedIndex = 0;
		} else {
			UISelectedIndex--;
			if (UISelectedIndex < 0) UISelectedIndex = UIMaxIdx;
		}
		bus.Publish<UIItemHighlighted, SelectionIdx>(new(UISelectedIndex));
	}
	public void transitionTo(QuizState nextState) {
		if (currentState == nextState) return;
		var oldState = currentState;
		currentState = nextState;
		bus.Publish<QuizStateTransition, QuizStateTransitionArgs>(new(oldState, nextState));
		bus.Publish<Debug, DebugVar>(new("state", currentState));
	}
}
namespace QuizSpace {
	public enum QuizState {
		START,
		SHOW_QUIZ_QUESTION,
		AWAIT_PLAYER_CHOICE,
	}
	public class UIItemHighlighted : TEvent<SelectionIdx> { }
	public class SelectionIdx(int index) : Args {
		public int index { get; init; } = index;
	}
	public class UIChoiceSelected : TEvent<SelectionIdx> { }
	public class QuizStateTransition : TEvent<QuizStateTransitionArgs> { }
	public class QuizStateTransitionArgs(QuizState from, QuizState to) : Args {
		public QuizState from { get; init; } = from;
		public QuizState to { get; init; } = to;
	}
}	
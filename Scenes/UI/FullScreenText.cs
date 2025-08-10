using Godot;
using System;
using AnimationEvents;
using InkBridge;
using OBus;
using UIEvents;

public partial class FullScreenText : Control {
	public Bus bus;
	public GlobalState global;
	[Export]
	public FullScreenTextHolder LabelContainer {get; set;} 
	[Export]
	public Label ChoiceLabel {get; set;}
	[Export]
	public PanelContainer ChoiceContainer {get; set;}
	
	private const string INACTIVE_TAG = "general";
	private const string FULLSCREEN_ACTIVE_TAG = "laur";
	private Color default_modulate; 
	bool active => global.QuizState.currentTag == FULLSCREEN_ACTIVE_TAG;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		default_modulate = ChoiceContainer.Modulate;
		bus.Subscribe<IShowDialogChoices, ChoiceDialogArgs>(args => {
			if (!active) return;
			LabelContainer.Append(args.content);
			if (args.choices.Count > 0) {
				ChoiceLabel.Text = args.choices[0].Text.Substring(0,8);
			} else {
				ChoiceLabel.Text = ">";
			}
		});
		bus.Subscribe<InkTagUpdated, InkTag>(args => {
			if (args.newTag == FULLSCREEN_ACTIVE_TAG) {
				LabelContainer.Reset();
			}
		});
		bus.Subscribe<AnimationFinished, AnimationName>(arg => {
			if (arg.name == "WhiteOutFrom3DSpace") {
				animate_button_in();
			} else if (arg.name == "DissolveFromFullscreen") {
				animate_button_out();
			}
		});
	}

	void animate_button_in() => animate_button(new Color("ffffff"));
	void animate_button_out() => animate_button(new Color("ffffff00"));
	void animate_button(Color color) {
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(ChoiceContainer, "modulate", color, 1.0f);
	}
}

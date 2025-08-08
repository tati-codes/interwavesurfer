using Godot;
using System;
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

	private const string INACTIVE_TAG = "general";
	private const string FULLSCREEN_ACTIVE_TAG = "laur";
	bool active => global.QuizState.currentTag == FULLSCREEN_ACTIVE_TAG;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
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
	}
}

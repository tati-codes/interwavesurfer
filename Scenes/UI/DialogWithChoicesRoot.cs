using OBus;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotInk;
using InkBridge;
using Interactables;
using OBus;
using UIEvents;
[GlobalClass]
public partial class DialogWithChoicesRoot : PanelContainer {
	public Bus bus;
	public GlobalState global;
	[ExportGroup("Internal")]
	[Export]
	public Label NameHolder {get; set;} 
	[Export]
	public ChoiceDialogTextHolder ContentContainer {get; set;} 
	[Export]
	public DialogChoicesContainer ChoiceContainer {get; set;}
  
	private const string ACTIVE_TAG = "general";
	bool active => global.QuizState.currentTag == ACTIVE_TAG || global.QuizState.currentTag == "calculate";

	public override void _Ready() {
		this.Hide();
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		bus.Subscribe<ChoiceSelected, IChoice>(args => {
			ContentContainer.Append("[i][color=bbbc95]You: [b]" + args.escapedText + "[/b][/color] [/i]");
		});
		bus.Subscribe<IShowDialogChoices, ChoiceDialogArgs>(consume);
		bus.Subscribe<InkTagUpdated, InkTag>(args => {
			if (args.newTag == "general") {
				this.Show();
				ContentContainer.Reset();
			}
		});
	}

	public void consume(ChoiceDialogArgs args) {
			if (!active) return;
			NameHolder.Text = args.title;		
			if (args.choices.Count == 0) ContentContainer.Maximize();
			else {
				ContentContainer.Minimize();
				ContentContainer.Reset();
			}
			ContentContainer.Append(args.content);
			ChoiceContainer.Consume(args.choices);
	}
	public void force(ChoiceDialogArgs args) {
		NameHolder.Text = args.title;		
		if (args.choices.Count == 0) ContentContainer.Maximize();
		else {
			ContentContainer.Minimize();
			ContentContainer.Reset();
		}
		ContentContainer.Append(args.content);
		ChoiceContainer.Consume(args.choices);
	}
}

namespace UIEvents {
	public class IShowDialogChoices: TEvent<ChoiceDialogArgs> {}
	public class ChoiceDialogArgs(string _content,  List<InkChoice> _choices, string _title) : Args {
		public string content { get; init; } = _content;
		public List<InkChoice> choices { get; init; } = _choices;
		public string title { get; init; } = _title;
	}
}

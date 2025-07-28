using OBus;
using Godot;
using System;
using Interactables;
using OBus;
using UIEvents;
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

	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		bus.Subscribe<IShowDialogChoices, ChoiceDialogArgs>(args => {
				NameHolder.Text = args.title;		
				ContentContainer.Append(args.content);
				ChoiceContainer.Consume(args.choices);
		});
	}
}

namespace UIEvents {
	public class IShowDialogChoices: TEvent<ChoiceDialogArgs> {}
	public class ChoiceDialogArgs(string _content, string[] _choices, string _title) : Args {
		public string content { get; init; } = _content;
		public string[] choices { get; init; } = _choices;
		public string title { get; init; } = _title;
	}
}

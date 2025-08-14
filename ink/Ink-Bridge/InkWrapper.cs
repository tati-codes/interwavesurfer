using Godot;
using GodotInk;
using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using InkBridge;
using OBus;
using Tag = Ink.Parsed.Tag;

[GlobalClass]
public partial class     InkWrapper : Node {
	[Export]
	public InkStory story;
	private Bus bus;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<SelectChoice, IChoice>(args => {
			story.ChooseChoiceIndex(args.choice.Index);
			story.Continue();
		});
		bus.Subscribe<SuperSelect, IChoice>(args => {
			story.ChooseChoiceIndex(args.choice.Index);
			if (story.CanContinue) {
				story.Continue();
			}
			if (story.CanContinue) {
				story.Continue();
			}
		});

		bus.Subscribe<PlayerContinuedStory>(args => story.Continue());
		bus.Subscribe<SuperContinue>(args => story.ContinueMaximally());
		story.Continued += () => {
			//FIXME story can stop being able to continue due to being over
			trackTag();
			if (!story.CanContinue && story.CurrentChoices.Count > 0) bus.Publish<ChoiceRequired, InkChoices>(new(story.CurrentChoices, story.CurrentText) {level = LOG_LEVEL.DETAILED});
			else if (!story.CanContinue && story.CurrentChoices.Count == 0) {
				GD.Print(story.CurrentText);
				bus.Publish<StoryUpdated, StoryText>(new (story.CurrentText));
				bus.Error("Story over//Implement this");
			}
			else if (story.CurrentText.Length < 2) {
				story.Continue();
			} else {
				bus.Publish<StoryUpdated, StoryText>(new(story.CurrentText));
			}
		};
		//TODO "reset quiz" event 
		//TODO reset quiz implementation
		story.MadeChoice += (InkChoice choice) => bus.Publish<ChoiceSelected, IChoice>( new(choice));
	}

	private void trackTag() {
		if (story.CurrentTags.Count == 0) return;
		if (story.CurrentTags.Count > 1) {
			bus.Inspect(story.CurrentTags);
			throw new Exception("Unexpected amount of tags in ink story.");
		}
		string newTag =  story.CurrentTags[0];
		if (this.tag == newTag) return;
		else if (this.tag != newTag) {
			this.tag = newTag;
			bus.Publish<InkTagUpdated, InkTag>(new(this.tag));
		}
	}

	public string tag = string.Empty;
	public string currentText => story.CurrentText;
	public bool canContinue => story.CanContinue;
	public IReadOnlyList<InkChoice> CurrentChoices => story.CurrentChoices;
	public void Continue() => story.Continue();

	public void getQuizVariables() {
		int navigation = story.FetchVariable<int>("navigation");
		int seaworthy = story.FetchVariable<int>("seaworthy");
		int rudderwork = story.FetchVariable<int>("rudderwork");
		int doldrums = story.FetchVariable<int>("doldrums");
		int starboard = story.FetchVariable<int>("starboard");
		int portside = story.FetchVariable<int>("portside");
		int salt = story.FetchVariable<int>("salt");
		List<int> vars = new List<int>() {navigation, rudderwork, doldrums, starboard, portside, salt, seaworthy};
		var sorted = vars.OrderByDescending(x => x).ToList();
		story.StoreVariable<int>("highest_stat", sorted.Max());
		story.StoreVariable<int>("lowest_stat", sorted.Min());
		int high = story.FetchVariable<int>("highest_stat");
		int low = story.FetchVariable<int>("lowest_stat");
		bus.Log($"High: {high}, Low: {low}");
	}
}

namespace InkBridge {
	public class InkTagUpdated : TEvent<InkTag> { }
	public class InkTag(string tag) : Args {
		public string newTag { get; init; } = tag;
	}
	public class ChoiceSelected : TEvent<IChoice> { }
	// public class PlayerContinued : TEvent<NArgs> {}
	public class StoryUpdated : TEvent<StoryText> {}
	public class SuperContinue : TEvent<NArgs> {}
	public class SelectChoice : TEvent<IChoice> {}
	public class SuperSelect : TEvent<IChoice> {}

	public class StoryText(string text) : Args {
		public string Text { get; init; } = text;
	}
	public class InkChoices(List<InkChoice> choices, string _line) : Args {
		public string line { get; init; } = _line;
		public List<InkChoice> choices { get; init; } = choices;
		public InkChoices(IReadOnlyList<InkChoice> choices, string _line) : this(choices.ToList(), _line) { }
	}

	public class PlayerContinuedStory : TEvent<NArgs> { }

	public class IChoice(InkChoice choice) : Args {
		public InkChoice choice { get; init; } = choice;
		public bool hasDirective => choice.Text.Contains(':');

		public string directive {
			get {
				if (choice.Text.Contains(':')) {
					var processed = choice.Text.Split(":");
					return processed[0] ?? "ERROR";
				} else {
					return choice.Text;
				}
			}
		}

		public string escapedText {
			get {
				if (choice.Text.Contains(':')) {
					var processed = choice.Text.Split(":");
					return processed[1] ?? "ERROR";
				} else {
					return choice.Text;
				}
			}
		}
	}
	public class ChoiceRequired: TEvent<InkChoices> {} //I chose this name because Subscribe<Choices... and its inverse seem really explicit
}

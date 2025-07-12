using Godot;
using GodotInk;
using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using InkBridge;
using OBus;
[GlobalClass]
public partial class InkWrapper : Node {
	[Export]
	public InkStory story;
	private Bus bus;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		story.ResetState();
		story.Continued += () => {
			bus.Publish<StoryUpdated, StoryText>(new(story.CurrentText));
			if (!story.CanContinue) bus.Publish<Choices, InkChoices>(new(story.CurrentChoices) {level = LOG_LEVEL.DETAILED});
		};
		story.MadeChoice += (InkChoice choice) => bus.Publish<>(choice.Text);
		story.ContinueMaximally();
		story.ChooseChoiceIndex(0);
	}
}

namespace InkBridge {
	public class PlayerContinued : TEvent<NArgs> {}
	public class StoryUpdated : TEvent<StoryText> {}
	public class SelectChoice : TEvent<IChoice> {}
	public class StoryText(string text) : Args {
		public string Text { get; init; } = text;
	}
	public class InkChoices(List<InkChoice> choices) : Args {
		public List<InkChoice> choices { get; init; } = choices;
		public InkChoices(IReadOnlyList<InkChoice> choices) : this(choices.ToList()) { }
	}
	public class IChoice(InkChoice choice) : Args {
		public InkChoice choice { get; init; } = choice;
	}
	public class Choices: TEvent<InkChoices> {} //I chose this name because Subscribe<Choices... and its inverse seem really explicit
}

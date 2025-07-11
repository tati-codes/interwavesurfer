using Godot;
using GodotInk;
using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using OBus;

public partial class InkWrapper : Node {
	[Export]
	public InkStory story;
	[Export] 
	public Button button;
	private Bus bus;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		story.Continued += () => bus.Publish<StoryUpdated, StoryText>(new(story.CurrentText));
		story.ResetState();
		bus.Log($"CurrentText: {story.CurrentText}\n");
		story.MadeChoice += (InkChoice choice) => bus.Log(choice.Text);
		story.ContinueMaximally();
		button.Pressed += buttonPush;
	}
	//DONE ChooseChoice busEvent
	//DONE PropagateChoices busEvent
	public void buttonPush() {
		bus.Log($"CurrentText: {story.CurrentText}\n" +
		        $"CurrentChoices: {String.Join(", ", story.CurrentChoices.Select((choice => choice.Text)))}\n" +
		        $"HasWarning: {story.HasWarning}\n" +
		        $"HasError: {story.HasError}\n" +
		        $"CanContinue: {story.CanContinue}\n" +
		        $"CurrentFlowName: {story.CurrentFlowName}\n");
		
		if (story.CurrentChoices.Count > 0) {
			foreach (InkChoice choice in story.CurrentChoices) {
				bus.Publish<SelectChoice, InkChoiceArgs>(new(){ choice = choice });
			}
		}
		if (story.CurrentTags.Count > 0) {
			foreach (var tag in story.CurrentTags) {
				bus.Log(tag);
			}
		}
		bus.Log("Can Continue:", story.CanContinue.ToString());
	}
}

namespace InkBridge {
	public class InkChoices : Args {
		public List<InkChoice> choices;
	}
	public class SelectChoice: TEvent<InkChoiceArgs> {}
	public class Choices: TEvent<InkChoices> {} //I chose this name because Subscribe<Choices... and its inverse seem really explicit
		
}

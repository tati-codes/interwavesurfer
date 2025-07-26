using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotInk;
using InkBridge;
using OBus;
using QuizSpace;

public partial class ChoiceScene : Node3D {
	Bus bus;
	[Export]
	public MeshInstance3D Text {get; set;} 
	[Export]
	public MeshInstance3D Portal {get; set;} 
	[Export]
	public int storyIndex { get; set; }
	[Export]
	public Material textMaterial {get; set;}

	private string _placeholder = "";
	[Export]
	public string placeholder { get => _placeholder;  set => _placeholder = value; } 
	
	[Export]
	public SpotLight3D spotlight {get; set;}


	private Color defaultSpot = new("00ffff");
  private Color selectedSpot = new("bd1e49");
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<Choices, InkChoices>(onChoicesEvent);
		bus.Subscribe<QuizStateTransition, QuizStateTransitionArgs>(handleStateTransition);
		bus.Subscribe<UIItemHighlighted, SelectionIdx>(handleHighlight);
		bus.Subscribe<UIChoiceSelected, SelectionIdx>(handleSelect);
	}

	void handleSelect(SelectionIdx args) {
		if (args.index != storyIndex) return;
		bus.Publish<SailTowards,Location>(new(new(Portal.GlobalPosition.X, 0, Portal.GlobalPosition.Z)));
	}
	void handleHighlight(SelectionIdx args) {
		if (this.storyIndex == args.index) {
			this.spotlight.SetColor(this.selectedSpot);
			this.spotlight.Show();
		} else {
			this.spotlight.Hide();
		}
	}
	void handleStateTransition(QuizStateTransitionArgs args) {
		if (args.from == QuizState.SHOW_QUIZ_QUESTION && args.to == QuizState.AWAIT_PLAYER_CHOICE) {
			spotlight.Show();
			spotlight.SetColor(defaultSpot);
		}
	}
	void replaceText(string newText) {
		var tMesh = new TextMesh();
		var processed = newText.Split(":");
		tMesh.Text = processed[1] ?? "ERROR";
		tMesh.FontSize = 128;
		tMesh.Material = textMaterial;
		Text.Mesh = tMesh;
	}
	void onChoicesEvent( InkChoices args ) {
		if (args.choices.All(choice => choice.Index != this.storyIndex)) {
			// bus.Log($"No choice with ID: {this.storyIndex} found. Hiding.");
			this.Hide();
			return;
		}
		foreach (InkChoice choice in args.choices) {
			if (choice.Index == this.storyIndex) {
				// bus.Log($"Choice with ID: {this.storyIndex} found.");
				this.Show();
				replaceText(choice.Text);
			}
		}
	}
}


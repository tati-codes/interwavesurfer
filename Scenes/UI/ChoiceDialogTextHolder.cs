using Godot;
using System;
using OBus;
public partial class ChoiceDialogTextHolder : ScrollContainer {
	[Export]
	public VBoxContainer Container {get; set;}
	[Export]
	public TextureRect Arrow {get; set;} 
	[Export]
	public PackedScene labelScene {get; set;} 
	private Godot.Vector2 small = new(845, 100);
	private Godot.Vector2 big = new(845, 200);

	public void Append(string text) {
		AppearingLabel newLabel = labelScene.Instantiate<AppearingLabel>();
		Container.AddChild(newLabel);
		newLabel.setText(text);
		if (Container.GetChildren().Count > 4) {
			Container.GetChild(0).QueueFree();
		}
	}

	public void Reset() {
		foreach (Node node in Container.GetChildren()) {
			node.QueueFree();
		}
	}

	public void Maximize() {
		CustomMinimumSize = big;
		Arrow.Show();
	}
	public void Minimize() {
		// CustomMinimumSize = small;
		Arrow.Hide();
	}
	
	public int Count {get => Container.GetChildren().Count;}
}

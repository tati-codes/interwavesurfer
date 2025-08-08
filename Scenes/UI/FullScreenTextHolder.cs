using Godot;
using System;
using OBus;
public partial class FullScreenTextHolder : VBoxContainer {
	[Export]
	public LabelSettings mainText {get; set;} 
	 
  
	public void Append(string text) {
		Label newLabel = new Label();
		newLabel.LabelSettings = mainText;
		newLabel.AutowrapMode = TextServer.AutowrapMode.Word;
		newLabel.Material = this.Material;
		AddChild(newLabel);
		newLabel.SetText(text);
		newLabel.HorizontalAlignment = HorizontalAlignment.Center;
		if (GetChildren().Count > 3) {
			GetChild(0).QueueFree();
		}
	}

	public void Reset() {
		foreach (Node node in GetChildren()) {
			node.QueueFree();
		}
	}
}

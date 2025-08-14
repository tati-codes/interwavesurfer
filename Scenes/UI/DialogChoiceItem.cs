using Godot;
using System;
using GodotInk;
using OBus;
using QuizSpace;

public partial class DialogChoiceItem : Control {
	[Export]
	public TextureRect Arrow {get; set;} 
	[Export]
	public Button ChoiceText {get; set;}

	public bool isOn = true;
	public InkChoice currentChoice;
	private Bus bus;
	private SubscriptionHolder subscriptions = new();
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		ChoiceText.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		var UIItemSub = bus.Subscribe<UIItemHighlighted, SelectionIdx>(args => {
			if (currentChoice == null) return;
			bus.Log("Dialog Choice Item: " + currentChoice.Text + " " + currentChoice.Index + " this is on: " + isOn.ToString());
			if (args.index == currentChoice.Index && this.isOn) {
				this.Select();
			} else if (this.isOn) {
				this.Deselect();
			}
		});
		subscriptions.Add(UIItemSub);
	}

	bool refersToMe(InkChoice other) {
		if (other.Text == currentChoice.Text && other.Index == currentChoice.Index) return true;
		return false;
	}

	public void setChoice(InkChoice choice, bool Selected = false) {
		currentChoice = choice;
		if (choice.Text.Contains(":")) {
			var processed = choice.Text.Split(":");
			ChoiceText.Text = processed[1] ?? "ERROR";
		} else {
			ChoiceText.Text = choice.Text;
		}
		if (Selected) Select();
		else Deselect();
	}
	public void Off() {
		isOn = false;
		Deselect();
		Arrow.Hide();
		ChoiceText.Text = string.Empty;
		this.currentChoice = null;
	}
	public void Select() {
		isOn = true;
		Arrow.SetInstanceShaderParameter("sway", true);
		Arrow.Show();
		ChoiceText.GrabFocus();
	}
	public void Deselect() {
		isOn = true;
		Arrow.SetInstanceShaderParameter("sway", false);
		Arrow.Show();
		ChoiceText.ReleaseFocus();
	}

	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
}


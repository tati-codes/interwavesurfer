using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using GodotInk;
using InkBridge;
using QuizSpace;

public partial class ChoiceContainer : VBoxContainer {
	public Bus bus;
	
	[Export]
	public Godot.Collections.Array<PcChoice> choices { get; set; }
	public override async void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<ChoiceRequired, InkChoices>(async args => await onChoicesEvent(args));
	}

	private async Task<bool> onChoicesEvent(InkChoices args) {
		// if (choices.All(choice => choice.Index != this.idx)) {
		// 	// bus.Log($"No choice with ID: {this.idx} found. Hiding.");
		// 	transparentize();
		// 	return false;
		// }
		foreach (var choix in choices) {
			choix.handleHighlight(new SelectionIdx(0));
			InkChoice currentChoice = args.choices.FirstOrDefault(choice => choice.Index == choix.idx);
			if (currentChoice != null) {
				choix.opaquen();
				await choix.ChoiceLabel.setText(currentChoice.Text);
			} else {
				choix.transparentize();
			}
		}
		return true;
	}


}

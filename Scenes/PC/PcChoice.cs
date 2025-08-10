using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using GodotInk;
using InkBridge;
using OBus;
using QuizSpace;

public partial class PcChoice : HBoxContainer {
	public Bus bus;
	[Export]
	public PCAppearingLabel ChoiceLabel {get; set;} 
	[Export]
	public Godot.TextureRect arrow {get; set;} 
	[Export]
	public int idx {get; set;}

	private Color visible = new("ffffff");
	private Color hidden = new("ffffff00");
	// on not selected arrow transparent
	//put anim player
  
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<UIItemHighlighted, SelectionIdx>(handleHighlight);

	}

	public void handleHighlight(SelectionIdx obj) {
		if (obj.index == idx) this.arrow.Modulate = visible;
		else this.arrow.Modulate = hidden;
	}

	public void transparentize() => this.Modulate = hidden;
	public void opaquen() => this.Modulate = visible;
	private async Task<bool> onChoicesEvent(InkChoices args) {
		if (args.choices.All(choice => choice.Index != this.idx)) {
			// bus.Log($"No choice with ID: {this.idx} found. Hiding.");
			transparentize();
			return false;
		}
		foreach (var choice in args.choices.Where(choice => choice.Index == this.idx)) {
			opaquen();
			bus.Log("pre awaken");
			await ChoiceLabel.setText(choice.Text);
			bus.Log("post awaken");
		}
		handleHighlight(new SelectionIdx(0));
		return true;
	}
}


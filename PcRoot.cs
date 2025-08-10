using Godot;
using System;
using System.Threading.Tasks;
using InkBridge;
using OBus;
using QuizSpace;

public partial class PcRoot : Control {
	public Bus bus;
	public GlobalState global;
	[Export]
	public InkWrapper story {get; set;} 
	[Export]
	public VBoxContainer LabelContainer { get; set; }
	[Export]
	public PackedScene pcText {get; set;} 
	[Export]
  public PCAppearingLabel TitleLabel { get; set; }
	[Export]
	public Control choiceContainer {get; set;} 
	[Export]
	public PCAppearingLabel continuer {get; set;} 
	[Export]
	public Control continuer_arrow {get; set;}

	private bool animationLock = false;
  
  
	public override async void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		bus.Subscribe<StoryUpdated, StoryText>(async args => {
			if (story.tag == "title") {
				await TitleLabel.setText(args.Text.TrimEnd(), 0.5f);
			} else {
			 await appendTerminalText(args.Text);
			 if (story.canContinue) {
				 await showContinue();
			 }
			}
			// Output.Text += args.Text;
			// Output.AppendText(args.Text);
			// Output.ScrollToLine(1000);
		});
		bus.Subscribe<ChoiceRequired, InkChoices>(async args => {
				 await showChoices();
				 await appendTerminalText(args.line);
			// Output.AppendText(args.line);
				// Output.Clear();
			}
			// bus.Publish<IShowDialogChoices, ChoiceDialogArgs>(new(args.line, args.choices, "???"))
		);
		bus.Subscribe<ChoiceSelected, IChoice>(args => {
			var children = LabelContainer.GetChildren();
			foreach (var childr in children) {
				childr.QueueFree();
			}
		});
		await TitleLabel.setText("Home", 0.5f);
		story.Continue();
	}

	private async Task<PCAppearingLabel> appendTerminalText(string text) {
		animationLock = true;
		var newLabel = pcText.Instantiate<PCAppearingLabel>();
		LabelContainer.AddChild(newLabel);
		if (LabelContainer.GetChildren().Count > 4) LabelContainer.GetChild(0).QueueFree();
		await newLabel.setText(text, 2.0f);
		animationLock = false;
		return newLabel;
	}

	public async Task showChoices() {
		bus.Log("CHO "+story.CurrentChoices.Count.ToString());
		if (!story.canContinue) {
			choiceContainer.Show();
			continuer.Hide();
			continuer_arrow.Hide();
			await continuer.setText("");
			bus.Log("IS Arrow Visble " + continuer_arrow.Visible);
			if (continuer_arrow.Visible) {
				continuer_arrow.Hide();
			}
		}
	}

	private async Task showContinue() {
		bus.Log("CONT " + story.CurrentChoices.Count.ToString());
		if (story.canContinue) {
			choiceContainer.Hide();
			continuer.Show();
			await continuer.setText("Continue...");
			continuer_arrow.Show();
		} 
		bus.Log("IS Arrow Visble (cshowcont)_" + continuer_arrow.Visible);
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionReleased("primary")) handlePrimary();
		else if (@event.IsActionReleased("move_left") || @event.IsActionReleased("move_right")) handleDirections(@event);
	}
	void handlePrimary() {
		if (animationLock) return;
		if (story.canContinue) bus.Publish<PlayerContinuedStory>();
		else bus.Publish<SuperSelect, IChoice>(new IChoice(global.QuizState.selectedChoice));
	}
	void handleDirections(InputEvent @event) {
		bool eventIsRight = @event.IsActionReleased("move_right");
		bool eventIsLeft = @event.IsActionReleased("move_left");
		// if (global.QuizState.currentChoices.Count <= 0) return;
		if (eventIsLeft) handleUISelect(next: false);
		else if (eventIsRight) handleUISelect(next: true);
	}
	
	void handleUISelect(bool next) {
		// if (global.QuizState.selectedChoice != null &&
		//     global.QuizState.currentChoices.Count > 1) {
			int UISelectedIndex = global.QuizState.selectedChoice.Index; 
			int UIMaxIdx = global.QuizState.currentChoices.Count;
			if (!next) { //we do it this way because visually the array is laid as 021. dont worry about it
				UISelectedIndex++;
				if (UISelectedIndex >= UIMaxIdx) UISelectedIndex = 0;
			} else {
				UISelectedIndex--;
				if (UISelectedIndex < 0) UISelectedIndex = UIMaxIdx - 1;
			}
			bus.Publish<UIItemHighlighted, SelectionIdx>(new(UISelectedIndex));
		// }
	}
}

using Godot;
using System;
using Interactables;
using OBus;
using UIEvents;
[GlobalClass]
public partial class DialogRoot : PanelContainer {
	public Bus bus;
	public GlobalState global;
	[ExportGroup("Internal")]
	[Export]
	public Label Name {get; set;} 
  
	[Export]
	public Label Content {get; set;} 
	[Export]
	public TextureRect Arrow {get; set;} 
	[Export]
	public AnimationPlayer anim {get; set;}
	private SubscriptionHolder subscriptions = new();
	private bool visible = false;
	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		subscriptions.Add(
			bus.Subscribe<IShowDialog, DialogText>(setDialog),
			bus.Subscribe<ReadItem, ReadableItem>(args => {
				if (!visible) setDialog("P. Digal", args.text_id, true);
				else reset();
			}),
			bus.Subscribe<UITransitionEv, UITransition>(handleUITransition)
		);
		Resized += repositionArrow;
	}
	void handleUITransition(UITransition args) {
		if (args.from == UIState.DIALOG)  reset();
		else if (args.to == UIState.DIALOG) this.Show();
	}
	void repositionArrow() {
		var sizeY = Size.Y;
		var halfWay = Size.X / 2;
		Arrow.Position = new Vector2(sizeY, halfWay);
	}
	public void setDialog(string name, string content, bool isLast) {
		visible = true;
		anim.Play("show");
		Name.Text = name;
		Content.Text = content;
		Arrow.SetInstanceShaderParameter("Bouncing", true);
	}
	public void setDialog(DialogText dialogText) => setDialog(dialogText.name, dialogText.content, dialogText.isLast);

	void reset() {
		if (!visible) return;
		visible = false;
		anim.PlayBackwards("show");
		Name.Text = "";
		Content.Text = "";
	} 
}


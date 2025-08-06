using Godot;
using System;
using Interactables;
using OBus;
using UIEvents;

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

	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		bus.Subscribe<ReadItem, ReadableItem>(args => {
			setDialog("???", global.getStringByObjectID(args.text_id), true);
		});
		bus.Subscribe<IShowDialog, DialogText>(setDialog);
		bus.Subscribe<UITransitionEv, UITransition>(handleUITransition);
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
		Name.Text = name;
		Content.Text = content;
		Arrow.SetInstanceShaderParameter("Bouncing", true);
	}
	public void setDialog(DialogText dialogText) => setDialog(dialogText.name, dialogText.content, dialogText.isLast);
	void reset() => setDialog("", "", false);	
}


using Godot;
using System;
using OBus;
public partial class AppearingLabel : RichTextLabel {

	[Export]
	public AnimationPlayer anim {get; set;}
	public void setText(string text) {
		Text = text;
		anim.Play("Appear");
		anim.AnimationFinished += (args) => {
			this.Material = null;
		};
	}
}


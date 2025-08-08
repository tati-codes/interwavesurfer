using Godot;
using System;

public partial class AppearingFsText : Label {
	[Export]
	public AnimationPlayer anim {get; set;}

	public Material secondMaterial {
		get;
		set;
	} = null;
	public void setText(string text) {
		Text = text;
		anim.Play("Appear");
		anim.AnimationFinished += (args) => {
			this.Material = secondMaterial;
		};
	}
}

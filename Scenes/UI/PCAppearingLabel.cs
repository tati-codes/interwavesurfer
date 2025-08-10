using Godot;
using System;
using System.Threading.Tasks;
using OBus;
public partial class PCAppearingLabel : Label {
	public Bus bus;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		this.VisibleCharacters = 0;
	}

	public async Task setText(string text, float duration = 1.0f) {
		this.Text = text;
		this.VisibleCharacters = 0;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "visible_characters", text.Length, duration);
		await ToSignal(tween, Tween.SignalName.Finished);
	}
}


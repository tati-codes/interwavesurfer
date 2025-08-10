using Godot;
using System;
using AnimationEvents;
using InkBridge;
using OBus;
public partial class AudioRoot : Node {
	public Bus bus;
	private GlobalState global;
	[Export]
	public AnimationPlayer anim {get; set;} 
	[Export]
	public AudioStreamPlayer audio {get; set;}
  
	public override void _Ready()	{
		global = GetNode<GlobalState>("/root/Global");
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<PlayerContinuedStory>(_ => {
			if (global.QuizState.currentTag == "laur") {
				audio.PitchScale = new RandomNumberGenerator().RandfRange(0.8f, 1.5f);
				anim.Play("book-page");
				bus.Log(audio.PitchScale.ToString());
			}
		});
		anim.AnimationFinished += (name) => {
			if (name == "book-page") {
				audio.PitchScale = 1.0f;
			}
		};
	}

}


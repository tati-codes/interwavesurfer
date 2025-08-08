using Godot;
using System;
using System.Collections.Generic;
using InkBridge;
using OBus;
using QuizSpace;

public partial class AnimPlayer : AnimationPlayer {
	public Bus bus;
	public GlobalState global;
	private const string toD3 = "general";
	private const string toD2 = "laur";
	private List<string> waitinglist = new();
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		bus.Subscribe<ChoiceSelected, IChoice>(args => {
			if (global.QuizState.currentTag == "laur") return;
 			switch (args.choice.Index) {
				case 0:
					batchPlay("LeftChoice");
					break;
				case 2:
					batchPlay("CenterChoice");
					break;
				case 1:
					batchPlay("RightChoice");
					break;
				default:
					break;
			}
		});
		bus.Subscribe<InkTagUpdated, InkTag>(args => {
			if (args.newTag == toD3) {
				Play("DissolveFromFullscreen");
			} else if (args.newTag == toD2) {
				batchPlay("WhiteOutFrom3DSpace");
			}
		});
		this.AnimationFinished += (name => {
			if (waitinglist.Count > 0) {
				var next = waitinglist[0];
				waitinglist.RemoveAt(0);
				if (name == next) return;
				Play(next);
			}
		});
		this.AnimationFinished += (name => {
			if (name == "WhiteOutFrom3DSpace") {
				Play("BoatReset");
			}
		});
	}
	void batchPlay(string anim) {
		if (!IsPlaying()) Play(anim);
		else {
			waitinglist.Add(anim);
		}
	}
	
	
}

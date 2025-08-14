using Godot;
using System;
using System.Collections.Generic;
using AnimationEvents;
using InkBridge;
using OBus;
using QuizSpace;

public partial class AnimPlayer : AnimationPlayer {
	public Bus bus;
	public GlobalState global;
	private const string toD3 = "general";
	private const string toD2 = "laur";
	private string lastTag = "";
	private List<string> waitinglist = new();
	private SubscriptionHolder subscriptions = new();
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		var choiceSelSub = bus.Subscribe<ChoiceSelected, IChoice>(args => {
			if (global.QuizState.currentTag is "laur" or "skip") return;
			bus.Log("Choice: " + args.choice.Text + "\n Tag: " + global.QuizState.currentTag);
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
		var inkTagSub = bus.Subscribe<InkTagUpdated, InkTag>(args => {
			if (lastTag == "calculate" && args.newTag != lastTag) {
				lastTag = args.newTag;
				return;	
			} 
			if (args.newTag == toD3) {
				Play("DissolveFromFullscreen");
			} else if (args.newTag == toD2) {
				batchPlay("WhiteOutFrom3DSpace");
			}
			lastTag = args.newTag;
		});
		this.AnimationFinished += (name => {
			if (waitinglist.Count > 0) {
				var next = waitinglist[0];
				waitinglist.RemoveAt(0);
				if (name == next) return;
				Play(next);
			}
		});
		this.AnimationFinished += (name => { // do we need one for anything else?
			if (name == "WhiteOutFrom3DSpace") {
				Play("BoatReset");
			}
		});
		this.AnimationFinished += (name) => bus.Publish<AnimationFinished, AnimationName>(new(name));
		subscriptions.Add(choiceSelSub, inkTagSub);
	}
	void batchPlay(string anim) {
		if (!IsPlaying()) Play(anim);
		else {
			waitinglist.Add(anim);
		}
	}
	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
}

namespace AnimationEvents {
public class AnimationFinished : TEvent<AnimationName> { }
public class AnimationName(string name) : Args {
	public string name { get; init; } = name;
}
}
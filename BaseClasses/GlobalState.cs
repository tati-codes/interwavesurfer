#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotInk;
using Ink.Runtime;
using InkBridge;
using Interactables;
using OBus;
using QuizSpace;
using State;
using TatiDebug;
using UIEvents;

public partial class GlobalState : Node {
	private Bus bus;

	public bool PlayerCanInteract = true;
	public bool PlayerIsHoldingItem { get; private set;  } = false;
	public NodeRef InteractionTarget { get; private set;  } = null;
	public Quiz QuizState { get; private set; }
	public PC PCState { get; private set; }
	public sceneEnum currentScene = sceneEnum.PC;
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		QuizState = new(bus);
		PCState = new PC(bus);
		subscribeToQuizEvents();
		subscribeToPCEvents();
		bus.Subscribe<PickupItem, PickupableItem>(args => PlayerIsHoldingItem = true);
		bus.Subscribe<DropItem, DropItemArgs>(args => PlayerIsHoldingItem = false);
		bus.Subscribe<LookingAt<ReadableItem>,ReadableItem>(args => InteractionTarget = args);
		bus.Subscribe<LookingAt<PickupableItem>, PickupableItem>(args => InteractionTarget = args);
		bus.Subscribe<StoppedLookingAt, NodeRef>(args => InteractionTarget = null);
	}

	private void subscribeToPCEvents() {
		bus.Subscribe<StoryUpdated, StoryText>(PCState.update);
		bus.Subscribe<ChoiceRequired, InkChoices>(PCState.update);
		bus.Subscribe<UIItemHighlighted, SelectionIdx>(PCState.update);
		bus.Subscribe<InkTagUpdated, InkTag>(PCState.update);
	}

	public void subscribeToQuizEvents() {
		bus.Subscribe<StoryUpdated, StoryText>(QuizState.update);
		bus.Subscribe<ChoiceRequired, InkChoices>(QuizState.update);
		bus.Subscribe<UIItemHighlighted, SelectionIdx>(QuizState.update);
		bus.Subscribe<InkTagUpdated, InkTag>(QuizState.update);
	}
	public string getStringByObjectID(string objectID) => "NotImplemented";
}

namespace State {
public enum sceneEnum {
		QUIZ,
		PC,
		ISLAND,
		HOUSE
}
	public class Quiz {
		public string currentText = "";
		public string currentTag = "laur";
		// public string currentTag = string.Empty;
		public List<InkChoice> currentChoices = new List<InkChoice>();
		public InkChoice? selectedChoice = null;
		public Bus bus;
		public Quiz(Bus _bus) {
			bus = _bus;
		}
		public void broadcastDebug() =>  bus.Publish<DebugText, Text>(new Text(this.ToString()) { tag = TatiDebug.DebugData.debug_tag }) ;
		
		public void update(StoryText text) {
			this.currentText = text.Text;
			broadcastDebug();
		}
		public void update(SelectionIdx args) {
			if (currentChoices.Count > args.index) {
				selectedChoice = currentChoices[args.index];
			} else {
				bus.Error("Fucked up UI Selection: " + args.index);
				bus.Error("CurrentChoices: " + currentChoices.Count);
				throw new Exception("Fucked up UI selection");
			}
			broadcastDebug();
		}
		public void update(InkChoices args) {
			this.currentChoices = args.choices;
			this.currentText = args.line;
			this.selectedChoice = args.choices[0];
			broadcastDebug();
		}

		public void update(InkTag args) {
			this.currentTag = args.newTag;
		}
		public override string ToString() {
			return $"Quiz.CurrentText: {currentText}\n Quiz.CurrentChoices: {string.Join(" ,", currentChoices.Select(choice => $"{choice.Text} / {choice.Index}"))}\n Quiz.SelectedChoice: {selectedChoice?.Text + "/" + selectedChoice?.Index}";
		}
	}

	public class PC : InkState {
		public PC(Bus _bus) : base(_bus) { }
	}

	public class InkState {
		public string currentText = string.Empty;
		public string currentTag = string.Empty;
		// public string currentTag = string.Empty;
		public List<InkChoice> currentChoices = new List<InkChoice>();
		public InkChoice? selectedChoice = null;
		public Bus bus;
		public bool debug = false;
		public void update(InkChoices args) {
			this.currentChoices = args.choices;
			this.currentText = args.line;
			this.selectedChoice = args.choices[0];
			broadcastDebug();
		}
		public void update(StoryText text) {
			this.currentText = text.Text;
			broadcastDebug();
		}
		public void update(SelectionIdx args) {
			if (currentChoices.Count > args.index) {
				selectedChoice = currentChoices[args.index];
			} else {
				bus.Error("Fucked up UI Selection: " + args.index);
				bus.Error("CurrentChoices: " + currentChoices.Count);
				throw new Exception("Fucked up UI selection");
			}
			broadcastDebug();
		}
		// public void broadcastDebug() => 
		// 	(debug ? bus.Publish<DebugText, Text>(new Text(this.ToString()) { tag = TatiDebug.DebugData.debug_tag }) : return);
		public void broadcastDebug() { if (debug) bus.Publish<DebugText, Text>(new Text(this.ToString()) { tag = TatiDebug.DebugData.debug_tag }); }
		public void update(InkTag args) {
			this.currentTag = args.newTag;
		}
		
		public InkState(Bus _bus) {
			bus = _bus;
		}
	}
}


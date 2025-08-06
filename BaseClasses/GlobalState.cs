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
	public Bus bus;

	public bool PlayerCanInteract = true;
	public bool PlayerIsHoldingItem { get; private set;  } = false;
	public NodeRef InteractionTarget { get; private set;  } = null;
	public Quiz QuizState { get; private set; }
	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		QuizState = new(bus);
		bus.Subscribe<PickupItem, PickupableItem>(args => PlayerIsHoldingItem = true);
		bus.Subscribe<DropItem, DropItemArgs>(args => PlayerIsHoldingItem = false);
		bus.Subscribe<LookingAt<ReadableItem>,ReadableItem>(args => InteractionTarget = args);
		bus.Subscribe<LookingAt<PickupableItem>, PickupableItem>(args => InteractionTarget = args);
		bus.Subscribe<StoppedLookingAt, NodeRef>(args => InteractionTarget = null);
		bus.Subscribe<StoryUpdated, StoryText>(QuizState.update);
		bus.Subscribe<ChoiceRequired, InkChoices>(QuizState.update);
		bus.Subscribe<UIItemHighlighted, SelectionIdx>(QuizState.update);
	}
	public string getStringByObjectID(string objectID) => "NotImplemented";
}

namespace State {
	public class Quiz {
		public string currentText = "";
		public List<InkChoice> currentChoices = new List<InkChoice>();
		public InkChoice? selectedChoice = null;
		public Bus bus;
		public Quiz(Bus _bus) {
			bus = _bus;
		}
		public void broadcastDebug() => bus.Publish<DebugText, Text>(new Text(this.ToString()) { tag = TatiDebug.DebugData.debug_tag });
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
		public override string ToString() {
			return $"Quiz.CurrentText: {currentText}\n Quiz.CurrentChoices: {string.Join(" ,", currentChoices.Select(choice => $"{choice.Text} / {choice.Index}"))}\n Quiz.SelectedChoice: {selectedChoice?.Text + "/" + selectedChoice?.Index}";
		}
	}
}


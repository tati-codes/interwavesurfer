using Godot;
using System;
using System.Collections.Generic;
using GodotInk;
using QuizSpace;

public partial class DialogChoicesContainer : HBoxContainer {
  [Export]
  public DialogChoiceItem First {get; set;} 
  [Export]
  public DialogChoiceItem Second{get; set;} 
  [Export]
  public DialogChoiceItem Third {get; set;}

  DialogChoiceItem[] choices = new DialogChoiceItem[3];
  private Bus bus;
  private GlobalState globalState;
  public override void _Ready() {
    bus = GetNode<Bus>("/root/bus");
  }

  public void Consume(List<InkChoice> choices) {
    if (choices.Count == 0) {
      this.Hide();
      return;
    } else this.Show();
    switch (choices.Count) {
      case 1:
        First.Off();
        Third.Off();
        Second.setChoice(choices[0], true);
        break; 
      case 2:
        Second.Off();
        First.setChoice(choices[0], true);
        Third.setChoice(choices[1]);
        break;
      case 3:
        First.setChoice(choices[0], true);
        Second.setChoice(choices[1]);
        Third.setChoice(choices[2]);
        break;
      default:
        GD.PrintErr("DialogChoicesContainer: Invalid choice length");
        break;
    }
  }
}

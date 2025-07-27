using Godot;
using System;

public partial class DialogChoicesContainer : HBoxContainer {
  [Export]
  public DialogChoiceItem First {get; set;} 
  [Export]
  public DialogChoiceItem Second{get; set;} 
  [Export]
  public DialogChoiceItem Third {get; set;}
  public void Consume(string[] choices) {
    switch (choices.Length) {
      case 1:
        Second.setChoiceText(choices[0]);
        break; 
      case 2:
        Third.setChoiceText(choices[1]);
        First.setChoiceText(choices[0]);
        break;
      case 3:
        First.setChoiceText(choices[0]);
        Second.setChoiceText(choices[1]);
        Third.setChoiceText(choices[2]);
        break;
      default:
        GD.PrintErr("DialogChoicesContainer: Invalid choice length");
        break;
    }
  }
}

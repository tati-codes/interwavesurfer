using Godot;
using System;
using OBus;
using TatiDebug;
using Debug = System.Diagnostics.Debug;

[GlobalClass]
public partial class FaceButtonZone : Control {
  [Export]
  public FaceButton image {get; set;} 
  [Export]
  public Label label {get; set;} 
  [Export]
  public TextureRect Frame {get; set;} 
	 
  [Export]
  public AnimationPlayer anim {get; set;}

  public Action batched = null;	
  // Bus bus;

  public override void _Ready() {
    // bus = GetNode<Bus>("/root/bus");
    image.setButton(null);
    label.Text = "";
    // anim.AnimationFinished += args => {
    //   if (batched != null) {
    //     batched();
    //     batched = null;
    //   }
    // };
  }
  public void show(PlayerInput.Buttons button, string text) {
    anim.Play("ShowUp");
    image.setButton(button);
    label.Text = text;;
  }
  public void disappear() {
    anim.Play("GoAway");
  }
}
using Godot;
using System;
using System.Numerics;
using TatiDebug;
public partial class Player : CharacterBody3D {
	public Bus bus = null;
  [Export]
  public Skeleton3D skly {get; set;}
  [Export]
  public AnimationPlayer animations {get; set;}
  [Export]
  public float speed {get; set;} = 3.0f;
  [Export]
  public float walk_speed {get; set;} = 1.0f;
  [Export]
  public float rot_speed {get; set;} = 20f;
  [Export]
  public float look_sens {get; set;} = 0.01f;
  [Export]
  public Camera3D cam {get; set;} 
  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
  public float rotation = 0F;
  public void dbg(string n, Object o) => bus.IPub<DisplayDebugScreen, DebugVar>(new(n,o)); 
  public Godot.Vector3 lastDirection;
  public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
    skly.Rotation = new(skly.Rotation.X, -cam.Rotation.Y, skly.Rotation.Z); 
	}
  void processMoveInput(double delta) {
    float preserve_Y = Velocity.Y;
    Velocity = Godot.Vector3.Zero;
    var i = Input.GetVector("move_backwards", "move_forwards","move_right", "move_left") * speed;
    Velocity = i.X * -cam.Basis.Z + i.Y * -cam.Basis.X + new Godot.Vector3(0, preserve_Y, 0);
    Velocity = new(Velocity.X, preserve_Y, Velocity.Z);
    receiveFacingInput(delta);
  }
  void receiveFacingInput(double delta) {
    float forwards = Input.GetAxis("move_backwards", "move_forwards");
    float sideways = Input.GetAxis("move_right", "move_left");
    if (forwards == 0 && sideways == 0) return;
    float angle = MathF.Atan2(sideways, forwards);
    var newY = Mathf.LerpAngle(Rotation.Y, angle, delta * rot_speed);
    Rotation = new(Rotation.X, (float)newY, Rotation.Z);
  }
  public override void _Process(double delta) {
    Velocity = new(Velocity.X, Velocity.Y - gravity * (float)delta, Velocity.Z);
    processMoveInput(delta);
    if (Math.Abs(Velocity.Y) > 1){
      animations.Play("Fall");
    } else if (Velocity.X == 0 && Velocity.Z == 0) {
      animations.Play("Idle");
    } else if (Velocity.X != 0 || Velocity.Z != 0) {
      animations.Play("Sprint");
    }
    if (IsOnFloor()) {
      animations.PlaybackDefaultBlendTime = 1;
      animations.SpeedScale = 1;
    }
    MoveAndSlide();
  }
}

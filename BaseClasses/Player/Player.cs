using Godot;
using System;
using TatiDebug;
[GlobalClass]
public partial class Player : CharacterBody3D {
	public Bus bus = null;
  [Export]
  public Node3D rotationContainer {get; set;}
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
  [Export]
  public InteractingBeam beam {get;set;}
  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
  public float rotation = 0F;
  public void dbg(string n, Object o) => bus.IPub<Debug, DebugVar>(new(n,o)); 
  public Vector3 lastDirection;
  public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
    rotationContainer.RotateY(-cam.Rotation.Y); 
    // beam.Rotation = new(beam.Rotation.X, -cam.Rotation.Y, beam.Rotation.Z);
	}

  public void setCamera(Camera3D cam) {
    // this.cam = cam;
    // rotationContainer.RotateY(-cam.Rotation.Y);
  }
  void processMoveInput(double delta) {
    float preserve_Y = Velocity.Y;
    Velocity = Vector3.Zero;
    var i = Input.GetVector("move_backwards", "move_forwards","move_right", "move_left") * speed;
    Velocity = i.X * -cam.Basis.Z + i.Y * -cam.Basis.X + new Vector3(0, preserve_Y, 0);
    Velocity = new(Velocity.X, preserve_Y, Velocity.Z);
  }
  void faceMotionwards(double delta) {
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
    faceMotionwards(delta);
    if (Math.Abs(Velocity.Y) > 1){
      //I'm falling!
      //animations.Play("Fall");
    } else if (Velocity.X == 0 && Velocity.Z == 0) {
      animations.Play("Player|Idle");
    } else if (Velocity.X != 0 || Velocity.Z != 0) {
      animations.Play("Player|Run");
    }
    if (IsOnFloor()) {
      animations.PlaybackDefaultBlendTime = 1;
      animations.SpeedScale = 1;
    }
    if (Position.Y < -6.3) {
      GetTree().Quit();
    }
    MoveAndSlide();
    RenderingServer.GlobalShaderParameterSet("player_position", Position);    // ProjectSettings.
    // player_position 
  }
}

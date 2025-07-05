using Godot;
using System;
using System.Numerics;
public partial class Guy : CharacterBody3D {
	public Bus bus = null;
  [Export]
  public Node3D meshLook {get; set;}
  [Export]
  public Skeleton3D skly {get; set;}
  [Export]
  public Godot.Collections.Array<MeshInstance3D> mesh { get; set;}
  [Export]
  public AnimationPlayer animations {get; set;}
  [Export]
  public Node3D cameraHolder {get; set;}
  [Export]
  public RayCast3D camera_raycast {get; set;}
  [Export]
  public Camera3D cam {get; set;}
  
  [Export]
  public float speed {get; set;} = 3.0f;
  [Export]
  public float walk_speed {get; set;} = 1.0f;
  [Export]
  public float rot_speed {get; set;} = 1.0f;
  [Export]
  public float look_sens {get; set;} = 0.01f;
  [Export]
  public Label debug {get; set;}
  public float gravity = 9.8F;
  public float rotation = 0F;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
    GD.Print(bus);
    // gravity = ProjectSettings.GetSetting("physics/3d/default_gravity"); variant is annoying
    Input.MouseMode = Input.MouseModeEnum.Captured;
	}

  //TODO SEPARATE CAMERA STUFF FROM MOVEMENT STUFF
  public override void _Input(InputEvent @event) {
    if (@event.IsActionPressed("ui_cancel")){
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
    if (@event is InputEventMouseMotion mouseEvent) {
      cameraHolder.RotateY(mouseEvent.Relative.X * -look_sens);
      camera_raycast.RotateX(mouseEvent.Relative.Y * look_sens);
      camera_raycast.Rotation = new Godot.Vector3(Mathf.Clamp(camera_raycast.Rotation.X, -Mathf.Pi/4, MathF.PI/4), camera_raycast.Rotation.Y, camera_raycast.Rotation.Z);
      skly.RotateY((mouseEvent.Relative.X * look_sens) * -1);
      rotation = skly.GlobalRotationDegrees.Y;
    }
    if (@event.IsActionPressed("Q")) {
      bus.Log(skly.GlobalRotationDegrees.ToString());
    }
  }
  void receiveMoveInput(double delta) {
    float preserve_Y = Velocity.Y;
    Velocity = Godot.Vector3.Zero;
    float movement =Input.GetAxis("move_backwards", "move_forwards");
    float rotation = Input.GetAxis("move_right", "move_left");
    Velocity += cameraHolder.Transform.Basis.Z * movement * speed;
    RotateY(rot_speed * rotation * (float)delta);
    Velocity = new(Velocity.X, preserve_Y, Velocity.Z);
  }

  public override void _Process(double delta) {
    Velocity = new(Velocity.X, Velocity.Y - gravity * (float)delta, Velocity.Z);
    receiveMoveInput(delta);
    // debug.Text = $"Velocity: {Velocity.ToString()}"; 
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
    // if (camera_raycast.IsColliding()) {
    //   cam.GlobalPosition = camera_raycast.GetCollisionPoint();
    // } else { 
    //   cam.Position = new Godot.Vector3(0,0,-2);
    //   foreach (MeshInstance3D item in mesh) {
    //     item.Position = Position - new Godot.Vector3(0,0.05f, 0);
    //   }
    // }
  }
}


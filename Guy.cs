using Godot;
using System;
using System.Numerics;
public partial class Guy : CharacterBody3D {
	public Bus bus = null;
  [Export]
  public Node3D meshLook {get; set;}
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
  public float run_speed {get; set;} = 3.0f;
  [Export]
  public float jump_velocity {get; set;} = 5.0f;
  [Export]
  public float look_sens {get; set;} = 0.0f;
  
  public Variant gravity;
  Godot.Vector2 temp = new(0,0);
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
    GD.Print(bus);
    gravity = ProjectSettings.GetSetting("physics/3d/default_gravity");
    // Input.MouseMode = Input.MouseModeEnum.Captured;
	}
  public override void _Input(InputEvent @event) {
    if (@event is InputEventMouseMotion mouseEvent) {
      cameraHolder.RotateY(mouseEvent.Relative.X * -look_sens);
      camera_raycast.RotateX(mouseEvent.Relative.Y * look_sens);
      camera_raycast.Rotation = new Godot.Vector3(Mathf.Clamp(camera_raycast.Rotation.X, -Mathf.Pi/4, MathF.PI/4), camera_raycast.Rotation.Y, camera_raycast.Rotation.Z);
    }
  }

  public override void _Process(double delta) {
    var input = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards").Normalized() * speed;
    if (input.IsEqualApprox(temp)) {

    } else {
      bus.Log("b " + input.ToString());
      temp = input;
      bus.Log("Velocity", ": ", Velocity.ToString());
    }
    Velocity = new Godot.Vector3(-(input.X * speed), 0, -(input.Y * speed));
    foreach (MeshInstance3D item in mesh) {
      item.LookAt(Position - new Godot.Vector3(Velocity.X, 0, Velocity.Z));
      Rotation = new Godot.Vector3(item.Rotation.X, item.Rotation.Y, (float)Mathf.LerpAngle(item.Rotation.Z, item.Rotation.Y, delta * 5)); 

    }
    if (Velocity.Y == 0 && (Velocity.X != 0 || Velocity.Z != 0)) {
      animations.Play("Sprint");
    } else if (Velocity.Y > 0) {
      animations.Play("Fall");
    } else if (Velocity.IsZeroApprox()) {
      animations.Play("Idle");
    }
    if (IsOnFloor()) {
      animations.PlaybackDefaultBlendTime = 1;
      animations.SpeedScale = 1;
    }
    MoveAndSlide();
    if (camera_raycast.IsColliding()) {
      cam.GlobalPosition = camera_raycast.GetCollisionPoint();
    } else { 
      cam.Position = new Godot.Vector3(0,0,-2);
    foreach (MeshInstance3D item in mesh) {
      item.Position = Position - new Godot.Vector3(0,0.05f, 0);

    }
    }
  }
}


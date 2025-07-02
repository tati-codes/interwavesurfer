extends CharacterBody3D

@onready var mesh_look:Node3D = $"Mesh Look"
@onready var mesh:MeshInstance3D = $"normal-man-a/Skeleton3D/Mesh"
@onready var animations:AnimationPlayer = $AnimationPlayer

@onready var camera_holder:Node3D = $"Camera Holder"
@onready var camera_raycast:RayCast3D = $"Camera Holder/RayCast3D"
@onready var camera:Camera3D = $"Camera Holder/RayCast3D/Camera3D"

@export var speed = 1.0
@export var walk_speed = 1.0
@export var run_speed = 3.0
@export var jump_velocity = 5.0
@export var look_sensitivity = 0.01

var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func _ready(): Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
func _input(event: InputEvent):
 if event is InputEventMouseMotion:
  camera_holder.rotate_y(event.relative.x * -look_sensitivity)
  camera_raycast.rotate_x(event.relative.y * look_sensitivity)
  camera_raycast.rotation.x = clamp(camera_raycast.rotation.x, -PI/4, PI/4)

func _process(delta: float):
 #if Input.is_action_pressed("walk"): speed = walk_speed
 #else: speed = run_speed
 #
 var input = Input.get_vector("move_left","move_right","move_forward","move_backward").normalized() * speed
 
 velocity = input.x * -camera_holder.global_basis.x + input.y * -camera_holder.global_basis.z + Vector3(0, velocity.y, 0)
 
 if input:
  mesh_look.look_at(position - Vector3(velocity.x, 0, velocity.z))
  if velocity.y == 0:
   if speed == walk_speed: animations.play("WalkN")
   else: animations.play("RunN")
 elif velocity.y == 0: animations.play("IdleN")
 mesh.rotation.z = lerp_angle(mesh.rotation.z, mesh_look.rotation.y, delta * 5)
 
 if is_on_floor():
  animations.playback_default_blend_time = 1
  animations.speed_scale = 1
  if Input.is_action_just_pressed("jump"):
   velocity.y = jump_velocity
   animations.play("JumpN")
 else:
  velocity.y -= gravity * delta
  animations.playback_default_blend_time = 0.5
  animations.speed_scale = 1.3
 
 move_and_slide()
 
 if camera_raycast.is_colliding(): camera.global_position = camera_raycast.get_collision_point()
 else: camera.position = Vector3(0, 0, -2)
 camera.position.z -= 0.1
 mesh.position = position - Vector3(0, 0.05, 0)

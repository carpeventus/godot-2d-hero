extends CharacterBody2D

var gravity := ProjectSettings.get("physics/2d/default_gravity") as float
const JUMP_VELOCITY = -300.0
const MOVE_SPEED = 50.0

@onready var sprite: Sprite2D = $Sprite2D
@onready var animationPlayer : AnimationPlayer = $AnimationPlayer
var direction : float = 0

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	direction = Input.get_axis("move_left", "move_right")
	if is_on_floor():
		if is_zero_approx(direction):
			animationPlayer.play("player_idle")
		else: 
			animationPlayer.play("player_run")
	else: 
		animationPlayer.play("player_jump")
	
	if not is_zero_approx(direction):
		sprite.flip_h = direction < 0
	
func _physics_process(delta):
	if is_on_floor() and Input.is_action_just_pressed("jump"):
		velocity.y = JUMP_VELOCITY
	
	if not is_on_floor():
		velocity.y += gravity * delta
	velocity.x = direction * MOVE_SPEED;
	move_and_slide()
	

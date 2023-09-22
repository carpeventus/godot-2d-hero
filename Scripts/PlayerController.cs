using System.Diagnostics;
using Godot;

public partial class PlayerController : CharacterBody2D
{
	public float gravity = (float) ProjectSettings.GetSetting("physics/2d/default_gravity").AsDouble();
	
	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;
	
	private float direction;
	public float moveSpeed = 60f;
	public float jumpVelocity = -400f;
	public override void _Ready() {
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			velocity.Y = jumpVelocity;

		}
		velocity.X = direction * moveSpeed;
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Process(double delta) {
		direction = Input.GetAxis("move_left", "move_right");
		// Animation
		if (IsOnFloor()) {
			if (Mathf.IsZeroApprox(direction)) {
				_animationPlayer.Play("player_idle");
			} else {
				_animationPlayer.Play("player_run");
			}
		} else {
			_animationPlayer.Play("player_jump");
		}

		if (!Mathf.IsZeroApprox(direction)) {
			_sprite.FlipH = direction < 0;
		}
	}
}


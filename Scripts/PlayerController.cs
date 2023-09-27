using Godot;

public partial class PlayerController : CharacterBody2D {
	public float DefaultGravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity").AsDouble();


	#region StateMachine
	public StateMachine<PlayerState> StateMachine { get; private set; }
	public PlayerIdleState PlayerIdleState { get; private set; }
	public PlayerRunState PlayerRunState { get; private set; }
	public PlayerJumpState PlayerJumpState { get; private set; }
	public PlayerInAirState PlayerInAirState { get; private set; }
	public PlayerLandingState PlayerLandingState { get; private set; }
	#endregion

	public AnimationPlayer AnimationPlayer { get; private set; }
	public Timer CoyoteTimer { get; private set; }
	public Timer JumpDelayInputTimer { get; private set; }

	public float Direction { get; set; }

	public float CurrentGravity { get; set; }

	public float MoveSpeed { get; private set; } = 60f;

	public float JumpVelocity { get; private set; } = -400f;
	public float MoveAcceleration { get; private set; } =  100f/ 0.1f;
	public float InAirAcceleration { get; private set; } = 100f / 0.01f;
	
	private Sprite2D _sprite;
	
	private void InitStateMachine() {
		StateMachine = new StateMachine<PlayerState>();
		PlayerIdleState = new PlayerIdleState(StateMachine, this, "player_idle");
		PlayerRunState = new PlayerRunState(StateMachine, this, "player_run");
		PlayerJumpState = new PlayerJumpState(StateMachine, this, "player_jump_prepare");
		PlayerInAirState = new PlayerInAirState(StateMachine, this, "player_in_air");
		PlayerLandingState = new PlayerLandingState(StateMachine, this, "player_landing");
		StateMachine.InitState(PlayerIdleState);
	}

	public override void _Ready() {
		CurrentGravity = DefaultGravity;
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_sprite = GetNode<Sprite2D>("Sprite2D");
		CoyoteTimer = GetNode<Timer>("CoyoteTimer");
		JumpDelayInputTimer = GetNode<Timer>("JumpDelayInputTimer");
		InitStateMachine();
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (@event.IsActionPressed("jump")) {
			JumpDelayInputTimer.Start();
		}
	}

	public override void _PhysicsProcess(double delta) {
		StateMachine.CurrentState.PhysicsUpdate(delta);
	}
	
	public override void _Process(double delta) {
		StateMachine.CurrentState.LogicUpdate(delta);
	}

	public void FlipSprite(bool flip) {
		_sprite.FlipH = flip;
	}

}


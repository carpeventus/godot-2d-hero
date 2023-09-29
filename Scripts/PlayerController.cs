using Godot;
using Hero.Scripts.State;
using Hero.Scripts.State.Player;

public partial class PlayerController : CharacterBody2D {
	public float DefaultGravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity").AsDouble();


	#region StateMachine
	public StateMachine<PlayerState> StateMachine { get; private set; }
	public PlayerIdleState PlayerIdleState { get; private set; }
	public PlayerRunState PlayerRunState { get; private set; }
	public PlayerJumpState PlayerJumpState { get; private set; }
	public PlayerInAirState PlayerInAirState { get; private set; }
	public PlayerLandingState PlayerLandingState { get; private set; }
	public PlayerWallSlidingState PlayerWallSlidingState { get; private set; }
	public PlayerWallJumpState PlayerWallJumpState { get; private set; }
	#endregion

	public AnimationPlayer AnimationPlayer { get; private set; }
	public Timer CoyoteTimer { get; private set; }
	public Timer JumpDelayInputTimer { get; private set; }
	public RayCast2D HeadCheck { get; private set; }
	public RayCast2D FootCheck { get; private set; }

	public float InputDirection { get; set; }
	public float CurrentGravity { get; set; }

	[Export] public float MoveSpeed { get; private set; } = 60f;

	[Export] public float JumpVelocity { get; private set; } = -400f;
	[Export] public Vector2 WallJumpVelocity { get; private set; } = new Vector2(350, -300f);
	[Export] public float MoveAcceleration { get; private set; } =  100f/ 0.2f;
	[Export] public float InAirAcceleration { get; private set; } = 100f / 0.1f;
	[Export] public float FallMulti { get; private set; } = 4f;
	[Export] public float WallSlideThreshold { get; private set; } = 8f;
	
	private Node2D _spriteWrap;
	
	private void InitStateMachine() {
		StateMachine = new StateMachine<PlayerState>();
		PlayerIdleState = new PlayerIdleState(StateMachine, this, "player_idle");
		PlayerRunState = new PlayerRunState(StateMachine, this, "player_run");
		PlayerJumpState = new PlayerJumpState(StateMachine, this, "player_jump_prepare");
		PlayerInAirState = new PlayerInAirState(StateMachine, this, "player_in_air");
		PlayerLandingState = new PlayerLandingState(StateMachine, this, "player_landing");
		PlayerWallSlidingState = new PlayerWallSlidingState(StateMachine, this, "player_wall_sliding");
		PlayerWallJumpState = new PlayerWallJumpState(StateMachine, this, "player_in_air");
		StateMachine.InitState(PlayerIdleState);
	}

	public override void _Ready() {
		CurrentGravity = DefaultGravity;
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_spriteWrap = GetNode<Node2D>("SpriteWrap");
		CoyoteTimer = GetNode<Timer>("CoyoteTimer");
		JumpDelayInputTimer = GetNode<Timer>("JumpDelayInputTimer");
		HeadCheck = GetNode<RayCast2D>("SpriteWrap/HeadRayCastChecker");
		FootCheck = GetNode<RayCast2D>("SpriteWrap/FootRayCastChecker");
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
		_spriteWrap.Scale = flip ? new Vector2(-1, 1) : new Vector2(1, 1);
	}

	public bool IsCollidingFront() {
		return HeadCheck.IsColliding() && FootCheck.IsColliding();
	}

	public bool IsWallDirectionEqualsInputDirection() {
		float wallCollidingDirection = GetWallNormal().X;
		return (wallCollidingDirection < 0 && InputDirection > 0) || (wallCollidingDirection > 0 && InputDirection < 0);
	}
}


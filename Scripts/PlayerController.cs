using System;
using System.Linq;
using Godot;
using Godot.Collections;

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
	public PlayerAttack1State PlayerAttack1State { get; private set; }
	public PlayerAttack2State PlayerAttack2State { get; private set; }
	public PlayerAttack3State PlayerAttack3State { get; private set; }
	public PlayerHurtState PlayerHurtState { get; private set; }
	public PlayerDieState PlayerDieState { get; private set; }
	
	public PlayerSlideStartState PlayerSlideStartState { get; private set; }
	public PlayerSlideLoopState PlayerSlideLoopState { get; private set; }
	public PlayerSlideEndState PlayerSlideEndState { get; private set; }
	#endregion

	public AnimationPlayer AnimationPlayer { get; private set; }
	public AnimatedSprite2D InteractFlag { get; private set; }
	public Timer CoyoteTimer { get; private set; }
	public Timer JumpDelayInputTimer { get; private set; }
	public Timer SlideDelayInputTimer { get; private set; }
	public Timer ImmuneTimer { get; private set; }
	public RayCast2D HeadCheck { get; private set; }
	public RayCast2D FootCheck { get; private set; }
	public HurtBox HurtBox { get; private set; }

	public float InputDirection { get; set; }
	public float CurrentGravity { get; set; }
	public bool IsAttackComboRequested { get; set; }
	[Export] public PackedScene GameOverScene;


	[Export] public float MoveSpeed { get; private set; } = 100f;

	[Export] public float JumpVelocity { get; private set; } = -330f;
	[Export] public float KnockBackForce { get; private set; } = 100f;
	[Export] public Vector2 WallJumpVelocity { get; private set; } = new Vector2(300, -300f);
	[Export] public float MoveAcceleration { get; private set; } =  100f/ 0.2f;
	[Export] public float InAirAcceleration { get; private set; } = 100f / 0.1f;
	[Export] public float FallMulti { get; private set; } = 3f;
	[Export] public float WallSlideThreshold { get; private set; } = 8f;
	[Export] public float SlideSpeed { get; private set; } = 80f;
	[Export] public bool CanCombo { get; private set; } = false;
	[Export] public float SlidingCostEnergy { get; private set; } = 4.0f;
	
	private Node2D _spriteWrap;
	public GameGlobal GameGlobal { get; private set; }
	public SoundManager SoundManager { get; private set; }

	public Dictionary<string, Interactable> InteractableWithDict { get; set; } = new();

	public Damage CurrentTakenDamage { get; private set; } = null;
	
	private void InitStateMachine() {
		StateMachine = new StateMachine<PlayerState>();
		PlayerIdleState = new PlayerIdleState(StateMachine, this, "player_idle");
		PlayerRunState = new PlayerRunState(StateMachine, this, "player_run");
		PlayerJumpState = new PlayerJumpState(StateMachine, this, "player_jump_prepare");
		PlayerInAirState = new PlayerInAirState(StateMachine, this, "player_in_air");
		PlayerLandingState = new PlayerLandingState(StateMachine, this, "player_landing");
		PlayerWallSlidingState = new PlayerWallSlidingState(StateMachine, this, "player_wall_sliding");
		PlayerWallJumpState = new PlayerWallJumpState(StateMachine, this, "player_in_air");
		PlayerAttack1State = new PlayerAttack1State(StateMachine, this, "player_attack_1");
		PlayerAttack2State = new PlayerAttack2State(StateMachine, this, "player_attack_2");
		PlayerAttack3State = new PlayerAttack3State(StateMachine, this, "player_attack_3");
		PlayerHurtState = new PlayerHurtState(StateMachine, this, "player_hurt");
		PlayerDieState = new PlayerDieState(StateMachine, this, "player_die");
		
		PlayerSlideStartState = new PlayerSlideStartState(StateMachine, this, "slide_start");
		PlayerSlideLoopState = new PlayerSlideLoopState(StateMachine, this, "slide_loop");
		PlayerSlideEndState = new PlayerSlideEndState(StateMachine, this, "slide_end");
		StateMachine.InitState(PlayerIdleState);
	}

	public override void _Ready() {
		CurrentGravity = DefaultGravity;
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_spriteWrap = GetNode<Node2D>("SpriteWrap");
		CoyoteTimer = GetNode<Timer>("CoyoteTimer");
		JumpDelayInputTimer = GetNode<Timer>("JumpDelayInputTimer");
		SlideDelayInputTimer = GetNode<Timer>("SlideDelayInputTimer");
		ImmuneTimer = GetNode<Timer>("ImmuneTimer");
		HeadCheck = GetNode<RayCast2D>("SpriteWrap/HeadRayCastChecker");
		FootCheck = GetNode<RayCast2D>("SpriteWrap/FootRayCastChecker");
		HurtBox = GetNode<HurtBox>("SpriteWrap/HurtBox");
		
		GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
		SoundManager = GetNode<SoundManager>("/root/SoundManager");
		InteractFlag = GetNode<AnimatedSprite2D>("InteractFlag");
		HurtBox.Hurt += OnHurt;
		InitStateMachine();
	}

	public void UpdateInteractFlagVisible() {
		InteractFlag.Visible = InteractableWithDict.Count > 0;
	}

	public void OnHurt(HitBox enemyHitBox) {
		// 无敌期间或已死亡
		if (IsPlayerImmuneNow() || IsDead())
		{
			return;
		}
		Damage damage = new Damage();
		damage.amount = 1;
		damage.source = enemyHitBox.Owner as Node2D;
		CurrentTakenDamage = damage;
		
		GameGlobal.Stat.TakeDamage(damage.amount);
		// 受伤直接由切换到受伤状态
		StateMachine.ChangeState(PlayerHurtState);
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (@event.IsActionPressed("jump")) {
			JumpDelayInputTimer.Start();
		}

		if (@event.IsActionPressed("attack") && CanCombo) {
			IsAttackComboRequested = true;
		}
		
		if (@event.IsActionPressed("slide")) {
			SlideDelayInputTimer.Start();
		}
		
		if (@event.IsActionPressed("interact") && InteractableWithDict.Count > 0) {
			InteractableWithDict.Last().Value.Interact();
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

	public void TakeDamage()
	{
		
		ImmuneTimer.Start();
		CurrentTakenDamage = null;
	}

	public bool IsPlayerImmuneNow()
	{
		return !ImmuneTimer.IsStopped();
	}
	
	public bool IsDead()
	{
		return GameGlobal.Stat.CurrentHealth <= 0;
	}

	public void Slide(double delta) {
		Vector2 v = Velocity;
		v.X = _spriteWrap.Scale.X * SlideSpeed;
		v.Y += (float) (CurrentGravity * delta);
		Velocity = v;
		MoveAndSlide();
	}

	public bool ShouldJump() {
		return (IsOnFloor() || CoyoteTimer.TimeLeft > 0) && JumpDelayInputTimer.TimeLeft > 0;
	}
	
	public bool ShouldSlide() {
		if (GameGlobal.Stat.CurrentEnergy < SlidingCostEnergy) {

			return false;
		}
		if (SlideDelayInputTimer.IsStopped()) {
			return false;
		}

		if (FootCheck.IsColliding()) {
			return false;
		}
		return true;
	}

	public void MoveToEntrypoint(Vector2 pos) {
		GlobalPosition = pos;
	}
}

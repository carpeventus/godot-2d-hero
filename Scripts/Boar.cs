using Godot;

public partial class Boar : Enemy {

    [Export] public float Acceleration = 100f / 0.2f;
    [Export] public float MoveSpeed = 50f;
    [Export] public float MaxSpeed = 80f;

    public BoarIdleState BoarIdleState { get; private set; }
    public BoarRunState BoarRunState { get; private set; }
    public BoarWalkState BoarWalkState { get; private set; }
    public StateMachine<BoarState> StateMachine { get; private set; }
    
    public RayCast2D PlayerCheck { get; private set; }
    public RayCast2D WallCheck { get; private set; }
    public RayCast2D FloorCheck { get; private set; }
    public Timer DelayStopRunTimer { get; private set; }
    public HurtBox HurtBox { get; private set; }
    
    private void InitStateMachine() {
        StateMachine = new StateMachine<BoarState>();
        BoarIdleState = new BoarIdleState(StateMachine, this, "boar_idle");
        BoarWalkState = new BoarWalkState(StateMachine, this, "boar_walk");
        BoarRunState = new BoarRunState(StateMachine, this, "boar_run");
        StateMachine.InitState(BoarIdleState);
    }

    public override void _Ready() {
        base._Ready();
        
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        WallCheck = GetNode<RayCast2D>("SpriteWrap/WallRayCastChecker");
        PlayerCheck = GetNode<RayCast2D>("SpriteWrap/PlayerRayCastChecker");
        FloorCheck = GetNode<RayCast2D>("SpriteWrap/FloorRayCastChecker");
        DelayStopRunTimer = GetNode<Timer>("DelayStopRunTimer");
        HurtBox = GetNode<HurtBox>("SpriteWrap/HurtBox");
        HurtBox.Hurt += OnHurt;
        InitStateMachine();
    }

    public bool CanNotMoveTowards() {
        return WallCheck.IsColliding() || !FloorCheck.IsColliding();
    }

    public override void _Process(double delta) {
        StateMachine.CurrentState.LogicUpdate(delta);
    }

    public override void _PhysicsProcess(double delta) {
        StateMachine.CurrentState.PhysicsUpdate(delta);
    }

    public override void Flip() {
        base.Flip();
        // 转向后立即强制更新碰撞信息
        WallCheck.ForceRaycastUpdate();
        PlayerCheck.ForceRaycastUpdate();
        FloorCheck.ForceRaycastUpdate();
    }

    public void OnHurt(HitBox hitBox) {
        GD.Print("攻击者" + hitBox.Owner.Name + "野猪受到伤害HP-10");

    }
}
using Godot;
using System;

public partial class Title : CanvasLayer
{
	[Export] private AudioStream Bgm;
	private Button _newGameButton;
	private Button _continueButton;
	private Button _exitButton;
	private VBoxContainer _boxContainer;
	public GameGlobal GameGlobal;
	public SoundManager SoundManager;
	private AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		GameGlobal = GetNode<GameGlobal>("/root/GameGlobal");
		SoundManager = GetNode<SoundManager>("/root/SoundManager");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_newGameButton = GetNode<Button>("%NewGame");
		_continueButton = GetNode<Button>("%Continue");
		_exitButton = GetNode<Button>("%Exit");
		_boxContainer = GetNode<VBoxContainer>("%VBoxContainer");
		_newGameButton.GrabFocus();
		_newGameButton.Pressed += OnNewGameButtonPressed; 
		_continueButton.Pressed += OnContinueButtonPressed; 
		_exitButton.Pressed += OnExitButtonPressed;
		_continueButton.Disabled = !GameGlobal.HasSaveFile();
		SoundManager.SetUpUiSound(this);
		SoundManager.PlayerBgm(Bgm);
	}
	
	public void OnNewGameButtonPressed()
	{
		GameGlobal.NewGame();
	}
	public void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
	public void OnContinueButtonPressed()
	{
		GameGlobal.LoadGame();
	}
}

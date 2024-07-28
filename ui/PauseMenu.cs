using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	public SoundManager SoundManager;
	private Button _quitButton;
	private Button _resumeButton;
	public override void _Ready()
	{
		SoundManager = GetNode<SoundManager>("/root/SoundManager");
		_quitButton = GetNode<Button>("%QuitButton");
		_resumeButton = GetNode<Button>("%ResumeButton");
		SoundManager.SetUpUiSound(this);
		_quitButton.Pressed += Quit;
		_resumeButton.Pressed += ResumeGame;
		_resumeButton.GrabFocus();
		GetTree().Paused = true;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			Close();
			GetTree().Root.SetInputAsHandled();
		}
	}

	public void Close()
	{
		GetTree().Paused = false;
		QueueFree();
	}
	public void ResumeGame()
	{
		Close();
	}

	public void Quit()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://title.tscn");
	}
}

using Godot;

public partial class SoundManager : Node
{
	private AudioStreamPlayer2D _bgmPlaer;

	private AudioStreamPlayer2D _uiSelectPlayer;
	private Node _sfx;

	public const string SFX_BUS_NAME = "Sfx";
	public const string MASTER_BUS_NAME = "Master";
	public const string MUSIC_BUS_NAME = "Music";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bgmPlaer = GetNode<AudioStreamPlayer2D>("BgmPlayer");
		_uiSelectPlayer =GetNode<AudioStreamPlayer2D>("SFX/UiSelect");
		_sfx =GetNode<Node>("SFX");
	}

	public void PlayerSfx(string nodeName)
	{

		var p = _sfx.GetNode<AudioStreamPlayer2D>(nodeName);
		if (p != null)
		{
			p.Play();
		}
	}

	public void PlayerBgm(AudioStream audioStream)
	{
		if (_bgmPlaer.Stream == audioStream && _bgmPlaer.IsPlaying())
		{
			return;
		}

		_bgmPlaer.Stream = audioStream;
		_bgmPlaer.Play();

	}
	
	public void SetUpUiSound(Node node)
	{
		if (node is Button button)
		{
			button.Pressed += () => _uiSelectPlayer.Play();
			button.MouseEntered += () => button.GrabFocus();
		}

		foreach (var n in node.GetChildren())
		{
			SetUpUiSound(n);
		}
	}

	public static float GetVolume(string busName)
	{
		int busIndex = AudioServer.GetBusIndex(busName);
		var db = AudioServer.GetBusVolumeDb(busIndex);
		return Mathf.DbToLinear(db);
	}
	
	public static void SetVolume(float percent, string busName)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		var db = Mathf.LinearToDb(percent);
		AudioServer.SetBusVolumeDb(busIndex, db);
	}
}

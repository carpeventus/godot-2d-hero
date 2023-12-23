using Godot;

public partial class Interactable : Area2D {

	[Signal]
	public delegate void InteractedEventHandler();
	public override void _Ready() {
		BodyEntered += _On_Area_Entered;
		BodyExited += _On_Area_Exited;
	}

	private void _On_Area_Exited(Node2D body) {
		if (body is PlayerController player) {
			player.InteractableWithDict.Remove(body.Name);
		}
	}

	private void _On_Area_Entered(Node2D body) {
		if (body is PlayerController player) {
			player.InteractableWithDict.Add(body.Name, this);
		}
	}

	public virtual void Interact() {
		EmitSignal(SignalName.Interacted);
	}
	
	
}

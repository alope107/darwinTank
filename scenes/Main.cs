using Godot;
using System;

public class Main : Node
{
	static PackedScene babyScene = GD.Load<PackedScene>("res://scenes/Baby.tscn");

	private int instanceCount = 0;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label instanceCountLabel = GetNode<Label>("InstanceCount");
		instanceCountLabel.Text = instanceCount.ToString();
	}

	public override void _Input(InputEvent @event)
	{
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
		{
			Baby babs = babyScene.Instance<Baby>();
			babs.Position = eventMouseButton.Position;
			AddChild(babs);
			instanceCount++;
			Label instanceCountLabel = GetNode<Label>("InstanceCount");
			instanceCountLabel.Text = instanceCount.ToString();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("force_up"))
		{ // TODO: rename action
			GD.Print("Hup");

			Baby babs = babyScene.Instance<Baby>();
			babs.Position = new Vector2((float)GD.RandRange(0, 300), (float)GD.RandRange(0, 300));
			AddChild(babs);
		}

		// if (Input.IsActionJustPressed("spawn_at_mouse"))
		// { // TODO: rename action
		// 	GD.Print("Yah");

		// 	Baby babs = babyScene.Instance<Baby>();
		// 	babs.Position = Get;
		// 	AddChild(babs);
		// }

		Label fpsDisplay = GetNode<Label>("FPSDisplay");
		fpsDisplay.Text = Engine.GetFramesPerSecond().ToString();
	}
}

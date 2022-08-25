using Godot;
using System;

public class Main : Node
{
	static PackedScene babyScene = GD.Load<PackedScene>("res://scenes/Baby.tscn");

	private int instanceCount = 0;

	public Main()
	{
		// Randomization placed in constructor instead of _Ready despite the docs
		// recommending the opposite. This is because _Ready is only called once
		// all of the child nodes have been initialized. If any of the child nodes
		// use random values (which Baby does) then the seed will not properly
		// have been set if the Randomize call is in _Ready.
		// The Main constructor is called before the children, so placing the call
		// here makes it work properly.
		// TODO: Be kind and make a PR to the Godot docs that changes their recommended
		// best practice.
		GD.Randomize();
	}

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

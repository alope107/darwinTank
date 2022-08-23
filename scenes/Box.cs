using Godot;
using System;

public class Box : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, -10000);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CanSleep = false; // just like me
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (Input.IsActionPressed("force_up"))
		{
			AppliedForce = thrust;
		}
		else
		{
			AppliedForce = Vector2.Zero;
		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}

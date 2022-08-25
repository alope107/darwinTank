using Godot;
using System;

public class Baby : RigidBody2D
{
	public override void _Ready()
	{
		createGeometry();
	}

	// TODO: Move to ConvexPolygon an convert to a nice toString
	private void printPoints(Vector2[] points)
	{
		GD.Print("My Points are:");
		foreach (Vector2 point in points)
		{
			GD.Print(point.x, " ", point.y);

		}
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		// This may cause issues when camera zoom etc
		// Thrust is applied with a global orientation
		// A better solution would  probably be to get the local vector then rotate
		// it back by the current rotation.
		Vector2 mousePos = GetGlobalMousePosition() - Position;
		Vector2 thrust = mousePos / 50;
		AppliedForce = thrust;


		float angle = mousePos.Angle();
		if (angle > Math.PI)
		{
			angle -= 2 * (float)Math.PI;
		}

		float normalizedTorque = angle / (float)Math.PI;

		AppliedTorque = normalizedTorque * 300;
	}

	private void addDrawLines(Vector2[] points)
	{
		for (int i = 0; i < points.Length; i++)
		{
			Line2D line = new Line2D();
			line.DefaultColor = new Color(0, 0, 0);
			line.Points = new Vector2[] { points[i], points[(i + 1) % points.Length] };
			line.Width = 2;
			AddChild(line);
		}
	}

	private void createGeometry()
	{
		ConvexPolygon polygonData = ConvexPolygon.RandomPolygon(new Vector2(-10, -10),
										 new Vector2(10, 10),
										10);

		Vector2[] points = polygonData.Points;

		var geometry = new Polygon2D();
		geometry.Color = new Color(GD.Randf(), GD.Randf(), GD.Randf());
		geometry.Polygon = points;
		AddChild(geometry);

		CollisionShape2D collider = new CollisionShape2D();
		ConvexPolygonShape2D polygon = new ConvexPolygonShape2D();
		polygon.Points = points;
		collider.Shape = polygon;
		AddChild(collider);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
	}
}

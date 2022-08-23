using Godot;
using System;

public class Baby : RigidBody2D
{

	private Line2D forceLine;

	public override void _Ready()
	{
		createGeometry();
		forceLine = new Line2D();
		forceLine.Width = 1;
		forceLine.Points = new Vector2[] { new Vector2(0, 0), new Vector2(0, 0) };
		AddChild(forceLine);
	}
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

		// GD.Print(thrust);

		float angle = mousePos.Angle();
		if (angle > Math.PI)
		{
			angle -= 2 * (float)Math.PI;
		}

		float normalizedTorque = angle / (float)Math.PI;

		AppliedTorque = normalizedTorque * 300;
	}

	private void createGeometry()
	{
		GD.Randomize();

		ConvexPolygon polygonData = ConvexPolygon.RandomPolygon(new Vector2(-10, -10),
										 new Vector2(10, 10),
										10);

		Vector2[] points = polygonData.Points;

		for (int i = 0; i < points.Length; i++)
		{
			// Line2D line = new Line2D();
			// line.DefaultColor = new Color(0, 0, 0);
			// line.Points = new Vector2[] { points[i], points[(i + 1) % points.Length] };
			// line.Width = 2;
			// AddChild(line);



			// Label num = new Label();
			// num.Text = i.ToString();
			// num.SetPosition(points[i]);
			// AddChild(num);

			// Line2D centerLine = new Line2D();
			// centerLine.DefaultColor = new Color(1, 1, 1);
			// centerLine.Points = new Vector2[] { center, points[i] };
			// centerLine.Width = 1;
			// AddChild(centerLine);

			// Line2D zeroLine = new Line2D();
			// zeroLine.DefaultColor = new Color(1, 0, 0);
			// zeroLine.Points = new Vector2[] { Vector2.Zero, points[i] };
			// zeroLine.Width = 1;
			// AddChild(zeroLine);
		}

		printPoints(points);
		var geometry = new Polygon2D();
		geometry.Color = new Color(GD.Randf(), GD.Randf(), GD.Randf());
		geometry.Polygon = points;
		AddChild(geometry);

		CollisionShape2D collider = new CollisionShape2D();
		ConvexPolygonShape2D polygon = new ConvexPolygonShape2D();
		polygon.Points = points;
		collider.Shape = polygon;
		AddChild(collider);

		// var yah = new Polygon2D();
		// yah.Polygon = new Vector2[] {new Vector2(-1, -1),
		// 								  new Vector2(1, -1),
		// 								  new Vector2(1, 1),
		// 								  new Vector2(-1, 1)};
		// yah.Color = new Color(0, 1, 0);

		// AddChild(yah);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//forceLine.Points = new Vector2[] { new Vector2(0, 0), GetLocalMousePosition() };
	}
}

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

	public static float signedArea(Vector2[] polygon)
	{
		float area = 0;
		for (int i = 0; i < polygon.Length - 1; i++)
		{
			Vector2 current = polygon[i];
			Vector2 next = polygon[i + 1];
			area += current.x * next.y - next.x * current.y;
		}
		GD.Print(area / 2);
		return area / 2;
	}

	private Vector2 centroid(Vector2[] polygon)
	{
		float x = 0;
		float y = 0;
		float area = signedArea(polygon);
		for (int i = 0; i < polygon.Length - 1; i++)
		{
			Vector2 current = polygon[i];
			Vector2 next = polygon[i + 1];
			float factor2 = current.x * next.y - next.x * current.y;
			x += (current.x + next.x) * factor2;
			y += (current.y + next.y) * factor2;
		}

		Vector2 result = new Vector2(x / (6 * area), y / (6 * area));
		GD.Print("Centroid");
		GD.Print(result);
		return result;
	}

	// lies
	private Vector2 centerOfMass(Vector2[] points)
	{
		Vector2 center = Vector2.Zero;
		foreach (Vector2 point in points)
		{
			center += point;
		}
		center /= points.Length;
		return center;
	}

	private Vector2[] centerPoints(Vector2[] points)
	{
		Vector2[] centered = new Vector2[points.Length];

		Vector2 center = centroid(points);

		for (int i = 0; i < points.Length; i++)
		{
			centered[i] = points[i] - center;
		}

		return centered;
	}

	// Sorts in-place
	private void sortClockwise(Vector2[] points)
	{
		Array.Sort(points, (a, b) => (int)Math.Ceiling(a.Angle() - b.Angle()));
	}

	private Vector2[] randomPolygon(Vector2 minCoord, Vector2 maxCoord, int numPoints)
	{
		Vector2[] points = new Vector2[numPoints];
		for (int i = 0; i < numPoints; i++)
		{
			points[i] = new Vector2((float)GD.RandRange(minCoord.x, maxCoord.x),
									(float)GD.RandRange(minCoord.y, maxCoord.y));
		}

		points = Geometry.ConvexHull2d(points);
		return centerPoints(points);
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

		Vector2[] points = randomPolygon(new Vector2(-10, -10),
										 new Vector2(10, 10),
										10);

		printPoints(points);
		Vector2 center = centroid(points);

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

		// Godot.Collections.Array polys = Geometry.MergePolygons2d(points, new Vector2[] { });
		// GD.Print($"I have {polys.Count} polys");
		// foreach (Vector2[] poly in polys)
		// {
		printPoints(points);
		var geometry = new Polygon2D();
		geometry.Color = new Color(GD.Randf(), GD.Randf(), GD.Randf());
		geometry.Polygon = points;
		AddChild(geometry);
		// }
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

using Godot;
using System;

public class ConvexPolygon
{
    // TODO: investigate mutability concerns
    public Vector2[] Points { get; private set; }
    public float Area { get; private set; }

    public ConvexPolygon(Vector2[] points)
    {
        Vector2[] hull = Geometry.ConvexHull2d(points);
        this.Points = centerPoints(hull);
    }
    private float signedArea(Vector2[] polygon)
    {
        float area = 0;
        for (int i = 0; i < polygon.Length - 1; i++)
        {
            Vector2 current = polygon[i];
            Vector2 next = polygon[i + 1];
            area += current.x * next.y - next.x * current.y;
        }
        area /= 2;
        Area = Math.Abs(area);
        return area;
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
        return result;
    }

    private Vector2[] centerPoints(Vector2[] points, Vector2 center)
    {
        Vector2[] centered = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            centered[i] = points[i] - center;
        }

        return centered;
    }

    private Vector2[] centerPoints(Vector2[] points)
    {
        return centerPoints(points, centroid(points));
    }

    public static ConvexPolygon RandomPolygon(Vector2 minCoord, Vector2 maxCoord, int numPoints)
    {
        Vector2[] points = new Vector2[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector2((float)GD.RandRange(minCoord.x, maxCoord.x),
                                    (float)GD.RandRange(minCoord.y, maxCoord.y));
        }

        return new ConvexPolygon(points);
    }
}

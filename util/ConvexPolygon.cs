using Godot;
using System;
using System.Linq;

/// <summary>
/// ConvexPolygon models the geometry of a 2D convex polygon.
/// It is NOT a game node, it only performs calculations and holds
/// data for nodes to use.
/// </summary>
public class ConvexPolygon // TODO: Investigate class vs struct in C#
{

    // Vertices of the Polygon in counter-clockwise order.
    // Points are guaranteed to be centered around the origin.
    // The last point is the same as the first point.
    // TODO: investigate mutability concerns
    public Vector2[] Points { get; private set; }

    // Area of the polygon. Not signed (is always positive).
    public float Area { get; private set; }

    // Creates a ConvexPolygon given an array of points.
    // An origin-centered convex hull is fit to the input points.
    // The points of the hull will be the vertices of the new polygon.
    // Input points are not modified
    public ConvexPolygon(Vector2[] points)
    {
        Vector2[] hull = Geometry.ConvexHull2d(points);
        this.Points = centerPoints(hull);
    }

    // Calculates the signed area of a polygon described by the array of points.
    // The first and last point must be the same.
    // Points must either be either ordered clockwise or counterclockwise.
    // If they are ordered clockwise, the signed area will be negative.
    // If they are ordered counterclockwise, the sined area will be positive.
    // The instance's Area atrribute is set to the absolute value of the computed
    // signed area. 
    private float signedArea(Vector2[] polygon)
    {
        // Uses the "shoelace formula" to compute area
        // See https://en.wikipedia.org/wiki/Shoelace_formula
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

    // Calculates the centroid of a polygon described by the array of points.
    // The first and last point must be the same.
    // Points must either be either ordered clockwise or counterclockwise.
    private Vector2 centroid(Vector2[] polygon)
    {
        // Based on the formula described here:
        // https://en.wikipedia.org/wiki/Centroid#Of_a_polygon
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

        return new Vector2(x / (6 * area), y / (6 * area));
    }

    // Given points and a centroid, return a new array of points that
    // are translated so the centroid of the new points is at the origin.
    private Vector2[] centerPoints(Vector2[] points, Vector2 center)
    {
        Vector2[] centered = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            centered[i] = points[i] - center;
        }

        return centered;
    }

    // Given points return a new array of points that
    // are translated so the centroid of the new points is at the origin.
    private Vector2[] centerPoints(Vector2[] points)
    {
        return centerPoints(points, centroid(points));
    }

    // Alternate constructor for a ConvexPolygon. Will create a random ConvexPolygon within the specified
    // bounding box. The ConvexPolygon will have a number of distinct vertices in the range [3, maxVertices].
    // It will have vertices+1 points because the first and last points are the same.
    public static ConvexPolygon RandomPolygon(Vector2 minCoord, Vector2 maxCoord, int maxVertices)
    {
        Vector2[] points = new Vector2[maxVertices];
        for (int i = 0; i < maxVertices; i++)
        {
            points[i] = new Vector2((float)GD.RandRange(minCoord.x, maxCoord.x),
                                    (float)GD.RandRange(minCoord.y, maxCoord.y));
        }

        return new ConvexPolygon(points);
    }

    /// Creates a string with the points in parentheses.
    /// Example: "((-2.7, -3.3), (-0.7, -1.3), (3.3, 4.7), (-2.7, -3.3))"
    /// Rounded to one decimal place.
    public override string ToString()
    {
        return $"({String.Join(", ", Points.Select(point => $"({point.x:0.#}, {point.y:0.#})"))})";
    }
}

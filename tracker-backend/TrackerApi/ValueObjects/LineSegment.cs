using System.Numerics;
using TrackerApi.DTOs;

namespace TrackerApi.ValueObjects;

public class LineSegment
{
    public Vector2 Start { get; }
    public Vector2 End { get; }

    public LineSegment(Vector2 start, Vector2 end)
    {
        if (start == end)
            throw new ArgumentException("A line segment cannot have zero length.");

        Start = start;
        End = end;
    }

    public LineSegment(CoordinateDTO start, CoordinateDTO end)
    {
        Start = new Vector2(start.X, start.Y);
        End = new Vector2(end.X, end.Y);

        if (Start == End)
            throw new ArgumentException("A line segment cannot have zero length.");
    }

    /// <summary>
    /// Direction vector (End - Start)
    /// </summary>
    public Vector2 Direction => End - Start;

    /// <summary>
    /// Length of the segment
    /// </summary>
    public float Length => Vector2.Distance(Start, End);

    /// <summary>
    /// Squared length
    /// </summary>
    public float LengthSquared => Vector2.DistanceSquared(Start, End);

    /// <summary>
    /// Closest point on the segment to an external point
    /// </summary>
    public Vector2 ClosestPoint(Vector2 p)
    {
        var ab = End - Start;
        var ap = p - Start;

        var t = Vector2.Dot(ap, ab);

        var tNormalized = t / ab.LengthSquared();

        tNormalized = MathF.Max(0, MathF.Min(1, tNormalized));

        return Start + tNormalized * ab;
    }

    public float DistanceTo(Vector2 point)
    {
        var closest = ClosestPoint(point);

        return Vector2.Distance(point, closest);
    }

    /// <summary>
    /// Gets lines from a collection of coordinates
    /// </summary>
    /// <param name="coordinates">A collection of coordinates</param>
    /// <returns></returns>
    public static List<LineSegment> GetLinesFromCoordinates(IEnumerable<CoordinateDTO> coordinates)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        var points = coordinates.ToArray();

        if (points.Length < 2)
            return [];

        var lines = new List<LineSegment>(points.Length - 1);

        for (int i = 1; i < points.Length; i++)
        {
            lines.Add(new LineSegment(points[i - 1], points[i]));
        }

        return lines;
    }
}
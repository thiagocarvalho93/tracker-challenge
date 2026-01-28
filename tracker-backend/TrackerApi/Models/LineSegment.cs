using System.Numerics;

namespace TrackerApi.Models;

public class LineSegment
{
    public Coordinate InitialCoordinate { get; set; }
    public Coordinate FinalCoordinate { get; set; }
    public double? Slope { get; set; }
    public double Intercept { get; set; }

    public LineSegment(Coordinate initialCoordinate, Coordinate finalCoordinate)
    {
        InitialCoordinate = initialCoordinate;
        FinalCoordinate = finalCoordinate;

        CalculateSlope();
        CalculateIntercept();
    }

    private void CalculateSlope()
    {
        var yDiff = FinalCoordinate.Y - InitialCoordinate.Y;
        var xDiff = FinalCoordinate.X - InitialCoordinate.X;

        if (xDiff == 0)
            Slope = null;

        Slope = yDiff / xDiff;
    }

    private void CalculateIntercept()
    {
        if (Slope is null)
        {
            Intercept = InitialCoordinate.X;
        }
        Intercept = InitialCoordinate.Y - Slope.GetValueOrDefault() * InitialCoordinate.X;
    }

    public Coordinate GetNearestPoint(Coordinate coordinate)
    {
        if (Slope is null)
        {
            return new Coordinate(InitialCoordinate.X, coordinate.Y);
        }
        var x = (coordinate.X + Slope * (coordinate.Y - Intercept)) /
            (1 + Math.Pow(Slope.GetValueOrDefault(), 2));

        var y = Slope.GetValueOrDefault() * x + Intercept;

        var nearest = new Coordinate(x.GetValueOrDefault(), y.GetValueOrDefault());

        return nearest;
    }
}
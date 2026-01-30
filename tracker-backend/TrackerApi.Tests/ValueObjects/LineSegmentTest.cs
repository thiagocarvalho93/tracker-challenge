namespace TrackerApi.Tests.ValueObjects;

using System.Numerics;
using TrackerApi.DTOs;
using TrackerApi.ValueObjects;
using Xunit;

public class LineSegmentTest
{
    #region Constructors

    [Fact]
    public void Constructor_Vector2_ShouldSetStartAndEnd()
    {
        var start = new Vector2(0, 0);
        var end = new Vector2(3, 4);

        var segment = new LineSegment(start, end);

        Assert.Equal(start, segment.Start);
        Assert.Equal(end, segment.End);
    }

    [Fact]
    public void Constructor_Vector2_ShouldThrow_WhenZeroLength()
    {
        var point = new Vector2(1, 1);

        var ex = Assert.Throws<ArgumentException>(() =>
            new LineSegment(point, point));

        Assert.Contains("zero length", ex.Message);
    }

    [Fact]
    public void Constructor_CoordinateDTO_ShouldSetStartAndEnd()
    {
        var start = new CoordinateDTO(0, 0);
        var end = new CoordinateDTO(1, 1);

        var segment = new LineSegment(start, end);

        Assert.Equal(new Vector2(0, 0), segment.Start);
        Assert.Equal(new Vector2(1, 1), segment.End);
    }

    [Fact]
    public void Constructor_CoordinateDTO_ShouldThrow_WhenZeroLength()
    {
        var point = new CoordinateDTO(2, 2);

        Assert.Throws<ArgumentException>(() =>
            new LineSegment(point, point));
    }

    #endregion

    #region Properties

    [Fact]
    public void Direction_ShouldBeEndMinusStart()
    {
        var segment = new LineSegment(
            new Vector2(1, 2),
            new Vector2(4, 6));

        Assert.Equal(new Vector2(3, 4), segment.Direction);
    }

    [Fact]
    public void Length_ShouldBeCorrect()
    {
        var segment = new LineSegment(
            new Vector2(0, 0),
            new Vector2(3, 4));

        Assert.Equal(5f, segment.Length);
    }

    [Fact]
    public void LengthSquared_ShouldBeCorrect()
    {
        var segment = new LineSegment(
            new Vector2(0, 0),
            new Vector2(3, 4));

        Assert.Equal(25f, segment.LengthSquared);
    }

    #endregion

    #region ClosestPoint

    [Fact]
    public void ClosestPoint_ShouldReturnProjection_WhenPointIsAboveSegment()
    {
        var segment = new LineSegment(
            new Vector2(0, 0),
            new Vector2(10, 0));

        var point = new Vector2(5, 5);

        var closest = segment.ClosestPoint(point);

        Assert.Equal(new Vector2(5, 0), closest);
    }

    [Fact]
    public void ClosestPoint_ShouldReturnStart_WhenPointIsBeforeSegment()
    {
        var segment = new LineSegment(
            new Vector2(2, 0),
            new Vector2(10, 0));

        var point = new Vector2(0, 3);

        var closest = segment.ClosestPoint(point);

        Assert.Equal(segment.Start, closest);
    }

    [Fact]
    public void ClosestPoint_ShouldReturnEnd_WhenPointIsAfterSegment()
    {
        var segment = new LineSegment(
            new Vector2(0, 0),
            new Vector2(5, 0));

        var point = new Vector2(10, -2);

        var closest = segment.ClosestPoint(point);

        Assert.Equal(segment.End, closest);
    }

    #endregion

    #region DistanceTo

    [Fact]
    public void DistanceTo_ShouldReturnCorrectDistance()
    {
        var segment = new LineSegment(
            new Vector2(0, 0),
            new Vector2(10, 0));

        var point = new Vector2(5, 3);

        var distance = segment.DistanceTo(point);

        Assert.Equal(3f, distance);
    }

    [Fact]
    public void DistanceTo_ShouldReturnDistanceToStart_WhenOutsideSegment()
    {
        var segment = new LineSegment(
            new Vector2(2, 2),
            new Vector2(6, 2));

        var point = new Vector2(0, 2);

        var distance = segment.DistanceTo(point);

        Assert.Equal(2f, distance);
    }

    #endregion

    #region GetLinesFromCoordinates

    [Fact]
    public void GetLinesFromCoordinates_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            LineSegment.GetLinesFromCoordinates(null!));
    }

    [Fact]
    public void GetLinesFromCoordinates_ShouldReturnEmpty_WhenLessThanTwoPoints()
    {
        var coordinates = new[]
        {
            new CoordinateDTO(0, 0)
        };

        var lines = LineSegment.GetLinesFromCoordinates(coordinates);

        Assert.Empty(lines);
    }

    [Fact]
    public void GetLinesFromCoordinates_ShouldCreateLineSegments()
    {
        var coordinates = new[]
        {
            new CoordinateDTO(0, 0),
            new CoordinateDTO(3, 0),
            new CoordinateDTO(3, 4)
        };

        var lines = LineSegment.GetLinesFromCoordinates(coordinates);

        Assert.Equal(2, lines.Count);

        Assert.Equal(new Vector2(0, 0), lines[0].Start);
        Assert.Equal(new Vector2(3, 0), lines[0].End);

        Assert.Equal(new Vector2(3, 0), lines[1].Start);
        Assert.Equal(new Vector2(3, 4), lines[1].End);
    }

    #endregion
}

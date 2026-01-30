using System.Collections;
using System.Numerics;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class TrackerService(IPathRepository pathRepository) : ITrackerService
{
    private readonly IPathRepository _pathRepository = pathRepository;

    public async Task<Status> GetStatus(Coordinate coordinate)
    {
        var coordinates = (await GetPathCoordinates()).ToArray();

        if (coordinates.Length < 2)
            throw new InvalidOperationException("Path must contain at least two coordinates.");

        var lines = LineSegment.GetLinesFromCoordinates(coordinates);

        var point = new Vector2(coordinate.X, coordinate.Y);

        float minOffset = float.MaxValue;
        float station = 0f;
        float accumulatedLength = 0f;
        var offsetPoint = lines.First().Start;

        foreach (var line in lines)
        {
            var closestPoint = line.ClosestPoint(point);
            var distance = Vector2.Distance(point, closestPoint);

            if (distance <= minOffset)
            {
                minOffset = distance;
                station = accumulatedLength + Vector2.Distance(line.Start, closestPoint);
                offsetPoint = closestPoint;
            }

            accumulatedLength += line.Length;
        }

        return new Status(minOffset, station, new Coordinate(offsetPoint.X, offsetPoint.Y));
    }

    public async Task<StatusStatefulResponseDTO> GetStatusStateful(Coordinate coordinate, int currentLineIndex)
    {
        var coordinates = (await GetPathCoordinates()).ToArray();

        if (coordinates.Length < 2)
            throw new InvalidOperationException("Path must contain at least two coordinates.");

        var lines = LineSegment.GetLinesFromCoordinates(coordinates);

        if (currentLineIndex < 0 || currentLineIndex >= lines.Count)
            throw new ArgumentOutOfRangeException(nameof(currentLineIndex));

        var point = new Vector2(coordinate.X, coordinate.Y);

        // Current line calculations
        var currentLine = lines[currentLineIndex];
        var currentClosestPoint = currentLine.ClosestPoint(point);
        var currentDistance = currentLine.DistanceTo(point);

        var stationBeforeCurrent = lines
            .Take(currentLineIndex)
            .Sum(l => l.Length);

        var currentStation =
            stationBeforeCurrent +
            Vector2.Distance(currentLine.Start, currentClosestPoint);

        // If it's the last line, no comparison needed
        if (currentLineIndex == lines.Count - 1)
        {
            return new StatusStatefulResponseDTO(
                currentDistance,
                currentStation,
                new Coordinate(currentClosestPoint.X, currentClosestPoint.Y),
                currentLineIndex);
        }

        // Compare with next line
        var nextLineIndex = currentLineIndex + 1;
        var nextLine = lines[nextLineIndex];
        var nextClosestPoint = nextLine.ClosestPoint(point);
        var nextDistance = nextLine.DistanceTo(point);

        if (nextDistance <= currentDistance)
        {
            var nextStation =
                currentStation +
                Vector2.Distance(nextLine.Start, nextClosestPoint);

            return new StatusStatefulResponseDTO(
                nextDistance,
                nextStation,
                new Coordinate(nextClosestPoint.X, nextClosestPoint.Y),
                nextLineIndex);
        }

        return new StatusStatefulResponseDTO(
            currentDistance,
            currentStation,
            new Coordinate(currentClosestPoint.X, currentClosestPoint.Y),
            currentLineIndex);
    }

    public async Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        return await _pathRepository.GetPathCoordinates();
    }
}
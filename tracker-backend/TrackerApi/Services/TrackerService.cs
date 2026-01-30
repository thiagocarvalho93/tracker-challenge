using System.Numerics;
using Microsoft.Extensions.Options;
using TrackerApi.DTOs;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;
using TrackerApi.Utils;
using TrackerApi.ValueObjects;

namespace TrackerApi.Services;

public class TrackerService : ITrackerService
{
    private readonly IPathRepository _pathRepository;
    private readonly string _fileName;

    public TrackerService(
        IPathRepository pathRepository,
        IOptions<PathSettings> options)
    {
        _pathRepository = pathRepository;
        _fileName = options.Value.FileName;

        if (string.IsNullOrWhiteSpace(_fileName))
            throw new InvalidOperationException("Path file name is not configured.");
    }

    public async Task<StatusDTO> GetStatus(CoordinateDTO coordinate)
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

        return new StatusDTO(minOffset, station, new CoordinateDTO(offsetPoint.X, offsetPoint.Y));
    }

    public async Task<StatusStatefulDTO> GetStatusStateful(CoordinateDTO coordinate, int currentLineIndex)
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
            return new StatusStatefulDTO(
                currentDistance,
                currentStation,
                new CoordinateDTO(currentClosestPoint.X, currentClosestPoint.Y),
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

            return new StatusStatefulDTO(
                nextDistance,
                nextStation,
                new CoordinateDTO(nextClosestPoint.X, nextClosestPoint.Y),
                nextLineIndex);
        }

        return new StatusStatefulDTO(
            currentDistance,
            currentStation,
            new CoordinateDTO(currentClosestPoint.X, currentClosestPoint.Y),
            currentLineIndex);
    }

    public async Task<IEnumerable<CoordinateDTO>> GetPathCoordinates()
    {
        return await _pathRepository.GetPathCoordinates(_fileName);
    }
}
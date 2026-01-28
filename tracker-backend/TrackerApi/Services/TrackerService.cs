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

        foreach (var line in lines)
        {
            var closestPoint = line.ClosestPoint(point);
            var distance = Vector2.Distance(point, closestPoint);

            if (distance < minOffset)
            {
                minOffset = distance;
                station = accumulatedLength + Vector2.Distance(line.Start, closestPoint);
            }

            accumulatedLength += line.Length;
        }

        return new Status(minOffset, station);
    }

    public async Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        return await _pathRepository.GetPathCoordinates();
    }
}
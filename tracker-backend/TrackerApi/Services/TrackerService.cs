using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class TrackerService(IPathService pathService) : ITrackerService
{
    private readonly IPathService _pathService = pathService;

    public Task<TrackingInformation> GetTrackingInformation(Coordinate coordinate)
    {
        // TODO: Get path information

        // TODO: Calculate offset

        // TODO: Calculate station
        throw new NotImplementedException();
    }
}
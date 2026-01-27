using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class TrackerService(IPathRepository pathRepository) : ITrackerService
{
    private readonly IPathRepository _pathRepository = pathRepository;

    public Task<TrackingInformation> GetTrackingInformation(Coordinate coordinate)
    {
        // TODO: Get path information

        // TODO: Calculate offset

        // TODO: Calculate station
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        return await _pathRepository.GetPathCoordinates();
    }
}
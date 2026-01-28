using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class TrackerService(IPathRepository pathRepository) : ITrackerService
{
    private readonly IPathRepository _pathRepository = pathRepository;

    public async Task<Status> GetStatus(Coordinate coordinate)
    {
        var path = await GetPathCoordinates();
        // TODO: Calculate offset

        // TODO: Calculate station
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        return await _pathRepository.GetPathCoordinates();
    }
}
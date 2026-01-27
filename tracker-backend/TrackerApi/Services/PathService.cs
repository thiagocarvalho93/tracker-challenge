using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class PathService : IPathService
{
    public Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        // TODO: Load path coordinates from file
        throw new NotImplementedException();
    }
}
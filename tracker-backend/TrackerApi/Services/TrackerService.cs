using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services;

public class TrackerService : ITrackerService
{
    public Task<decimal> GetOffset(Coordinate coordinate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Coordinate>> GetPath()
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetStation(Coordinate coordinate)
    {
        throw new NotImplementedException();
    }
}
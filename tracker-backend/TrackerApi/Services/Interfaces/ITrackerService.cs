using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<Status> GetStatus(Coordinate coordinate);
    Task<IEnumerable<Coordinate>> GetPathCoordinates();
}
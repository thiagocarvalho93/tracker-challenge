using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<decimal> GetOffset(Coordinate coordinate);
    Task<decimal> GetStation(Coordinate coordinate);
    Task<IEnumerable<Coordinate>> GetPath();
}
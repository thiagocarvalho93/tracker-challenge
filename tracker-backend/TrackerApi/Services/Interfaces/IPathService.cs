using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces;

public interface IPathService
{
    Task<IEnumerable<Coordinate>> GetPathCoordinates();
}
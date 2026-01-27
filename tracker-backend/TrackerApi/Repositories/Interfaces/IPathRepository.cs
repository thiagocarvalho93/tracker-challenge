using TrackerApi.Models;

namespace TrackerApi.Repositories.Interfaces;

public interface IPathRepository
{
    Task<IEnumerable<Coordinate>> GetPathCoordinates();
}
using TrackerApi.DTOs;

namespace TrackerApi.Repositories.Interfaces;

public interface IPathRepository
{
    Task<IEnumerable<CoordinateDTO>> GetPathCoordinates(string fileName);
}
using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<Status> GetStatus(Coordinate coordinate);
    Task<StatusStatefulResponseDTO> GetStatusStateful(Coordinate coordinate, int currentLineIndex);
    Task<IEnumerable<Coordinate>> GetPathCoordinates();
}
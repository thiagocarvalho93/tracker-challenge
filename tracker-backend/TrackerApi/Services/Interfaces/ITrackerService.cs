using TrackerApi.DTOs;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<StatusDTO> GetStatus(CoordinateDTO coordinate);
    Task<StatusStatefulDTO> GetStatusStateful(CoordinateDTO coordinate, int currentLineIndex);
    Task<IEnumerable<CoordinateDTO>> GetPathCoordinates();
}
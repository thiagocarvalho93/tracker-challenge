using TrackerApi.DTOs;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<StatusDTO> GetStatus(CoordinateDTO coordinate);
    Task<StatusDTO> GetStatusWithLineTrack(CoordinateDTO coordinate);
    Task<IEnumerable<CoordinateDTO>> GetPathCoordinates();
}
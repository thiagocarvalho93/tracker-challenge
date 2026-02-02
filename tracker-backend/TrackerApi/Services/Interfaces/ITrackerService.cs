using TrackerApi.DTOs;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<StatusDTO> GetStatus(CoordinateDTO coordinate);
    Task<StatusDTO> GetStatusWithLineTrack(CoordinateDTO coordinate, int currentLineIndex = 0);
    Task<IEnumerable<CoordinateDTO>> GetPathCoordinates();
}
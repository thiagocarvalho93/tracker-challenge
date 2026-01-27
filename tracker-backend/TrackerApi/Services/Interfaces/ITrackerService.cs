using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces;

public interface ITrackerService
{
    Task<TrackingInformation> GetTrackingInformation(Coordinate coordinate);
}
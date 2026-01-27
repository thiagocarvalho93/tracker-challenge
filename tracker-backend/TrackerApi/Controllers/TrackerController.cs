using Microsoft.AspNetCore.Mvc;
using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

public class TrackerController(ITrackerService trackerService, IPathService pathService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;
    private readonly IPathService _pathService = pathService;

    [HttpGet("info")]
    public async Task<IActionResult> GetOffset([FromQuery] Coordinate coordinate)
    {
        return Ok(await _trackerService.GetTrackingInformation(coordinate));
    }

    [HttpGet("path-coordinates")]
    public async Task<IActionResult> GetPathCoordinates()
    {
        return Ok(await _pathService.GetPathCoordinates());
    }
}
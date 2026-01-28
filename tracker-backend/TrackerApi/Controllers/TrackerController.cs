using Microsoft.AspNetCore.Mvc;
using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

public class TrackerController(ITrackerService trackerService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus([FromQuery] Coordinate coordinate)
    {
        return Ok(await _trackerService.GetStatus(coordinate));
    }

    [HttpGet("path")]
    public async Task<IActionResult> GetPathCoordinates()
    {
        return Ok(await _trackerService.GetPathCoordinates());
    }
}
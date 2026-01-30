using Microsoft.AspNetCore.Mvc;
using TrackerApi.DTOs;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

public class TrackerController(ITrackerService trackerService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;

    [HttpGet("status")]
    public async Task<IActionResult> GetStatusLess([FromQuery] CoordinateDTO coordinate)
    {
        return Ok(await _trackerService.GetStatus(coordinate));
    }

    [HttpGet("status-stateful")]
    public async Task<IActionResult> GetStatusFul([FromQuery] CoordinateDTO coordinate, [FromQuery] int lineIndex)
    {
        return Ok(await _trackerService.GetStatusStateful(coordinate, lineIndex));
    }

    [HttpGet("path")]
    public async Task<IActionResult> GetPathCoordinates()
    {
        return Ok(await _trackerService.GetPathCoordinates());
    }
}
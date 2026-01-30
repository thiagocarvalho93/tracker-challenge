using Microsoft.AspNetCore.Mvc;
using TrackerApi.DTOs;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

public class TrackerController(ITrackerService trackerService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;

    [HttpGet("status")]
    public async Task<IActionResult> GetStatusStateless([FromQuery] CoordinateDTO coordinate)
    {
        return Ok(await _trackerService.GetStatus(coordinate));
    }

    [HttpGet("status-stateful")]
    public async Task<IActionResult> GetStatusStatefull([FromQuery] CoordinateDTO coordinate)
    {
        return Ok(await _trackerService.GetStatusStateful(coordinate));
    }

    [HttpGet("path")]
    public async Task<IActionResult> GetPathCoordinates()
    {
        return Ok(await _trackerService.GetPathCoordinates());
    }

    [HttpDelete("reset")]
    public async Task<IActionResult> ResetCurrentLine([FromServices] IStateService stateService)
    {
        stateService.ResetCurrentLine();

        return NoContent();
    }
}
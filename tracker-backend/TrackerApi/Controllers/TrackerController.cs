using Microsoft.AspNetCore.Mvc;
using TrackerApi.DTOs;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

public class TrackerController(ITrackerService trackerService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus([FromQuery] CoordinateDTO coordinate, bool stateful = true)
    {
        if (stateful)
        {
            return Ok(await _trackerService.GetStatusStateful(coordinate));
        }
        return Ok(await _trackerService.GetStatus(coordinate));
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
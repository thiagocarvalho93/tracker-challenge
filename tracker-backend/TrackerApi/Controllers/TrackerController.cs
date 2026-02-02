using Microsoft.AspNetCore.Mvc;
using TrackerApi.DTOs;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

[ApiController]
[Route("api")]
public class TrackerController(ITrackerService trackerService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus([FromQuery] CoordinateDTO coordinate, [FromQuery] bool trackLine = false, [FromQuery] int currentLineIndex = 0)
    {
        if (trackLine)
        {
            return Ok(await _trackerService.GetStatusWithLineTrack(coordinate, currentLineIndex));
        }
        return Ok(await _trackerService.GetStatus(coordinate));
    }

    [HttpGet("path")]
    public async Task<IActionResult> GetPathCoordinates()
    {
        return Ok(await _trackerService.GetPathCoordinates());
    }
}
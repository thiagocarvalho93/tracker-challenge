using Microsoft.AspNetCore.Mvc;
using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers;

public class TrackerController(ITrackerService trackerService) : ControllerBase
{
    private readonly ITrackerService _trackerService = trackerService;

    [HttpGet("path")]
    public async Task<IActionResult> GetPath()
    {
        return Ok(await _trackerService.GetPath());
    }

    [HttpGet("offset")]
    public async Task<IActionResult> GetOffset([FromQuery] Coordinate coordinate)
    {
        return Ok(await _trackerService.GetOffset(coordinate));
    }

    [HttpGet("station")]
    public async Task<IActionResult> GetStation([FromQuery] Coordinate coordinate)
    {
        return Ok(await _trackerService.GetStation(coordinate));
    }
}
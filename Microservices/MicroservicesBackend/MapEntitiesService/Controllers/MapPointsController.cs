using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapEntitiesService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MapPointsController : ControllerBase
{
    private readonly IMapPointsService _mapPointsService;
    private readonly ILogger<MapPointsController> _logger;

    public MapPointsController(IMapPointsService mapPointsService, ILogger<MapPointsController> logger)
    {
        _mapPointsService = mapPointsService;
        _logger = logger;
    }

    [HttpPost(Name = "SetNewMapPoint")]
    public async Task<ActionResult> SetNewMapPoint([FromBody] MapPointDto mapPointDto)
    {
        bool setNewMapPointWasSuccessful = await _mapPointsService.AddNewMapPointAsync(mapPointDto);

        if (setNewMapPointWasSuccessful)
        {
            _logger.LogInformation($"Added a new map point - Name:{mapPointDto.Name}, X: {mapPointDto.X}, Y: {mapPointDto.Y}");
            return Ok();
        }
        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to set a new Map Point");
    }
}
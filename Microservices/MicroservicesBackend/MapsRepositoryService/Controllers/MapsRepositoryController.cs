using MapsRepositoryService.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MapsRepositoryController : ControllerBase
    {
        private readonly ILogger<MapsRepositoryController> _logger;

        public MapsRepositoryController(ILogger<MapsRepositoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "UploadMap")]
        public async Task<ActionResult> UploadMap(MapDto map)
        {
            //await _mapPointsService.AddNewMapPointAsync(mapPointDto);
            //_logger.LogInformation($"Added a new map point - Name:{mapPointDto.Name}, X: {mapPointDto.X}, Y: {mapPointDto.Y}");
            return Ok(map);
        }

        [HttpDelete("{mapName}", Name = "DeleteMap")]
        public async Task<ActionResult> DeleteMap(string mapName)
        {
            //TODO: add the processing logic here
            return Ok();
        }

        [HttpPost(Name = "SetMissionMap")]
        public async Task<ActionResult> SetMissionMap(MissionMapDto missionMap)
        {
            //await _mapPointsService.AddNewMapPointAsync(mapPointDto);
            //_logger.LogInformation($"Added a new map point - Name:{mapPointDto.Name}, X: {mapPointDto.X}, Y: {mapPointDto.Y}");
            //TODO: add the processing logic here
            return Ok();
        }
    }
}

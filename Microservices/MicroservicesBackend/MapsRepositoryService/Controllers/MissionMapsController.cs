using Connectivity.Core;
using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MapsRepositoryService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MissionMapsController : ControllerBase
    {
        private readonly IPublisher _publisher;
        private readonly ILogger<MissionMapsController> _logger;
        private readonly IGetMissionMapNameQuery _getMissionMapNameQuery;
        private readonly ISetMissionMapCommand _setMissionMapCommand;

        public MissionMapsController(IPublisher publisher,
            ILogger<MissionMapsController> logger, 
            IGetMissionMapNameQuery getMissionMapNameQuery,
            ISetMissionMapCommand setMissionMapCommand)
        {
            _publisher = publisher;
            _logger = logger;
            _getMissionMapNameQuery = getMissionMapNameQuery;
            _setMissionMapCommand = setMissionMapCommand;
        }

        [HttpPost(Name = "SetMissionMap")]
        public async Task<ActionResult> SetMissionMap(MissionMapDto missionMap)
        {
            var missionMapWasSet = string.Empty;

            var setMissionMapWasSuccessful = await _setMissionMapCommand.SetMainMissionMapAsync(missionMap.MissionMapName);
            try
            {
                missionMapWasSet = JsonConvert.SerializeObject(missionMap);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured when trying to serialize Mission Map data for publishing, details {ex}");
            }
            var misionMapWasSetPublishWasSuccessful = await _publisher.PublishAsync(missionMapWasSet);

            if (!string.IsNullOrEmpty(missionMapWasSet) && setMissionMapWasSuccessful &&
                misionMapWasSetPublishWasSuccessful)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError, $"Error occurred while trying to set map {missionMap.MissionMapName} as main mission map");
        }

        [HttpGet(Name = "GetMissionMapName")]
        public async Task<ActionResult> GetMissionMapName()
        {
            var missionMapName = await _getMissionMapNameQuery.GetMissionMapNameAsync();
            if (!string.IsNullOrEmpty(missionMapName))
            {
                return Ok(missionMapName);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, $"Exception occurred occurred while trying to fetch mission map name");
        }
    }
}

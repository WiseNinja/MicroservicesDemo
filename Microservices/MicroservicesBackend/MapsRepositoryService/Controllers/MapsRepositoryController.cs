using Common.Connectivity.Enums;
using Connectivity;
using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Core.DTOs;
using MapsRepositoryService.Infrastructure.MinIO.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace MapsRepositoryService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MapsRepositoryController : ControllerBase
{
    private readonly ILogger<MapsRepositoryController> _logger;
    private readonly IInsertMapCommand _insertMapCommand;
    private readonly IDeleteMapCommand _deleteMapCommand;
    private readonly IGetAllMapsQuery _getAllMapsQuery;
    private readonly IGetMapDataQuery _getMapDataQuery;
    private readonly ISetMissionMapCommand _setMissionMapCommand;
    private readonly IPublisher _publisher;

    public MapsRepositoryController(ILogger<MapsRepositoryController> logger,
        IInsertMapCommand insertMapCommand,
        IDeleteMapCommand deleteMapCommand,
        IGetAllMapsQuery getAllMapsQuery,
        IGetMapDataQuery getMapDataQuery,
        ISetMissionMapCommand setMissionMapCommand,
        IPublisher publisher)
    {
        _logger = logger;
        _insertMapCommand = insertMapCommand;
        _deleteMapCommand = deleteMapCommand;
        _getAllMapsQuery = getAllMapsQuery;
        _getMapDataQuery = getMapDataQuery;
        _setMissionMapCommand = setMissionMapCommand;
        _publisher = publisher;
    }

    [HttpPost(Name = "UploadMap")]
    public async Task<ActionResult> UploadMap(MapDto map)
    {
        try
        {
            await _insertMapCommand.InsertMapAsync(map);
            _logger.LogInformation($"Uploaded a new map with name {map.Name}");
            return Ok(map);
        }
        catch (Exception ex)
        {
            _logger.LogError($"A general exception occurred while trying to upload a new map to the server, details: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "A general exception occurred while trying to upload a new map to server");
        }
    }

    [HttpDelete("{mapName}", Name = "DeleteMap")]
    public async Task<ActionResult> DeleteMap(string mapName)
    {
        try
        {
            await _deleteMapCommand.DeleteMapAsync(mapName);
            _logger.LogInformation($"Deleted a map with name {mapName}");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"A general exception occurred while trying to delete a map from the server, details: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "A general exception occurred while trying to delete a map from the server");
        }
    }

    [HttpPost(Name = "SetMissionMap")]
    public async Task<ActionResult> SetMissionMap(MissionMapDto missionMap)
    {
        try
        {
            await _setMissionMapCommand.SetMainMissionMapAsync(missionMap.Name);
            Message message = new Message
            {
                MessageId = Guid.NewGuid(),
                MessageType = MessageType.MissionMapSet,
                Payload = JsonConvert.SerializeObject(missionMap)
            };
            await _publisher.PublishAsync(message);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"A general exception occurred while to set map {missionMap.Name} as main mission map, details: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, $"A general exception occurred while to set map {missionMap.Name} as main mission map");
        }
    }

    [HttpGet(Name = "GetAllMapNames")]
    public async Task<ActionResult> GetAllMapNames()
    {
        try
        {
            List<string> mapNames =await  _getAllMapsQuery.GetAllMapNamesAsync();
            return Ok(mapNames);
        }
        catch (Exception ex)
        {
            _logger.LogError($"A general exception occurred while trying to get all map names, details: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "A general exception occurred while trying to get all map names");
        }
    }

    [HttpGet(Name = "GetMapDataByMapName")]
    public async Task<ActionResult> GetMapDataByMapName(string mapName)
    {
        try
        {
            string mapData = await _getMapDataQuery.GetMapDataByNameAsync(mapName);
            return Ok(mapData);
        }
        catch (Exception ex)
        {
            _logger.LogError($"A general exception occurred while trying to map data for map : {mapName}, details: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, $"A general exception occurred while trying to map data for map : {mapName}");
        }
    }

    [HttpGet(Name = "GetMissionMapData")]
    public async Task<ActionResult> GetMissionMapData(string mapName)
    {
        try
        {
          //  string mapData = await _getMapDataQuery.GetMapDataByNameAsync(mapName);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"A general exception occurred while trying to map data for map : {mapName}, details: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, $"A general exception occurred while trying to map data for map : {mapName}");
        }
    }

    [HttpGet(Name = "GetMissionMapName")]
    public async Task<ActionResult> GetMissionMapName()
    {
        try
        {
            //string mapData = await _getMapDataQuery.GetMapDataByNameAsync(mapName);
            return Ok();
        }
        catch (Exception ex)
        {
            //_logger.LogError($"A general exception occurred while trying to map data for map : {mapName}, details: {ex}");
            //return StatusCode(StatusCodes.Status500InternalServerError, $"A general exception occurred while trying to map data for map : {mapName}");
        }
    }
}
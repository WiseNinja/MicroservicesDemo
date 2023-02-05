using Connectivity.Core;
using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
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

    public MapsRepositoryController(ILogger<MapsRepositoryController> logger,
        IGetAllMapsQuery getAllMapsQuery,
        IGetMapDataQuery getMapDataQuery,
        IInsertMapCommand insertMapCommand,
        IDeleteMapCommand deleteMapCommand)
    {
        _logger = logger;
        _insertMapCommand = insertMapCommand;
        _deleteMapCommand = deleteMapCommand;
        _getAllMapsQuery = getAllMapsQuery;
        _getMapDataQuery = getMapDataQuery;
    }

    [HttpPost(Name = "UploadMap")]
    public async Task<ActionResult> UploadMap(MapDto map)
    {
        var mapUploadWasSuccessful = await _insertMapCommand.InsertMapAsync(map);
        if (mapUploadWasSuccessful)
        {
            _logger.LogInformation($"Uploaded a new map with name {map.Name}");
            return Ok(map);
        }
        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to upload a new map to server");
    }

    [HttpDelete("{mapName}", Name = "DeleteMap")]
    public async Task<ActionResult> DeleteMap(string mapName)
    {
        var mapDeletionWasSuccessful = await _deleteMapCommand.DeleteMapAsync(mapName);
        if (mapDeletionWasSuccessful)
        {
            _logger.LogInformation($"Deleted a map with name {mapName}");
            return Ok();
        }
        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to delete a map from the server");
    }

    [HttpGet(Name = "GetAllMapNames")]
    public async Task<ActionResult> GetAllMapNames()
    {
        var mapNames = await _getAllMapsQuery.GetAllMapNamesAsync();
        if (mapNames is not null)
        {
            return Ok(mapNames);
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "Exception occurred while trying to get all map names");
    }

    [HttpGet(Name = "GetMapDataByMapName")]
    public async Task<ActionResult> GetMapDataByMapName(string mapName)
    {
        var mapData = await _getMapDataQuery.GetMapDataByNameAsync(mapName);
        if (!string.IsNullOrEmpty(mapData))
        {
            return Ok(mapData);
        }
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error occurred while trying to map data for map : {mapName}");
    }
}
using AutoMapper;
using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using MapEntitiesService.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MapEntitiesService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MapPointsController : ControllerBase
{
    private readonly IMapPointsService _mapPointsService;
    private readonly IMapper _mapper;

    public MapPointsController(IMapPointsService mapPointsService, IMapper mapper)
    {
        _mapPointsService = mapPointsService;
        _mapper = mapper;
    }
    [HttpPost(Name = "SetNewMapPoint")]
    public async Task<ActionResult> SetNewMapPoint([FromBody] MapPointDto mapPointDto)
    {
        await _mapPointsService.AddNewMapPointAsync(mapPointDto);
        return Ok();
    }
}
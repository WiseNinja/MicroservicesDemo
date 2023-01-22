﻿using AutoMapper;
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
    private readonly ILogger<MapPointsController> _logger;

    public MapPointsController(IMapPointsService mapPointsService, IMapper mapper, ILogger<MapPointsController> logger)
    {
        _mapPointsService = mapPointsService;
        _mapper = mapper;
        _logger = logger;
    }
    [HttpPost(Name = "SetNewMapPoint")]
    public async Task<ActionResult> SetNewMapPoint([FromBody] MapPointDto mapPointDto)
    {
        await _mapPointsService.AddNewMapPointAsync(mapPointDto);
        _logger.Log(LogLevel.Information, $"Added a new map point - Name:{mapPointDto.Name}, X: {mapPointDto.X}, Y: {mapPointDto.Y}");
        return Ok();
    }
}
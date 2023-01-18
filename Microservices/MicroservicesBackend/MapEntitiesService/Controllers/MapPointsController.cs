using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using MapEntitiesService.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MapEntitiesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapPointsController : ControllerBase
    {
        private readonly IMapPointsService _mapPointsService;

        public MapPointsController(IMapPointsService mapPointsService)
        {
            _mapPointsService = mapPointsService;
        }
        [HttpPost(Name = "SetNewMapPoint")]
        public async Task<ActionResult> SetNewMapPoint([FromBody] MapPointVm mapPointVm)
        {
            MapPointDto mapPointDto = new MapPointDto();
            await _mapPointsService.AddNewMapPoint(mapPointDto);
            return Ok();
        }
    }
}

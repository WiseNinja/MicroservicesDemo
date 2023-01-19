using AutoMapper;
using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Entities;
using MapEntitiesService.Core.ViewModels;

namespace MapEntitiesService.Core.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<MapPointVm, MapPointDto>().ReverseMap();
        CreateMap<MapPoint, MapPointDto>().ReverseMap();
    }
}
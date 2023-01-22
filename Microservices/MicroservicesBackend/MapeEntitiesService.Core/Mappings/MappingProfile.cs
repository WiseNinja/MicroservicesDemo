using AutoMapper;
using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Entities;

namespace MapEntitiesService.Core.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<MapPoint, MapPointDto>().ReverseMap();
    }
}
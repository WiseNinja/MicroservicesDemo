using Newtonsoft.Json;

namespace EntitiesPresenter.DTOs;

public class EntityDetailsDto
{
    public string? Name { get; set; }
    [JsonRequired]
    public double X { get; set; }
    [JsonRequired]
    public double Y { get; set; }
}
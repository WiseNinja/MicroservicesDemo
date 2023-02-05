using System.ComponentModel.DataAnnotations;

namespace MapEntitiesService.Core.DTOs;

public class MapPointDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public double X { get; set; }
    [Required] 
    public double Y { get; set; }
}
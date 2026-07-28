using System.ComponentModel.DataAnnotations;

namespace FlightApi.Dto.Request;

public class UpdateAirportRequest
{
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string IataCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; } = "Vietnam";
}

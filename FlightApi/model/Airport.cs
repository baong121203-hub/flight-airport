using System.ComponentModel.DataAnnotations;

namespace FlightApi.Model;

public class Airport
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(3)]
    public string IataCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; } = "Vietnam";

    public ICollection<Flight> OriginFlights { get; set; } = new List<Flight>();
    public ICollection<Flight> DestinationFlights { get; set; } = new List<Flight>();
}

using System.ComponentModel.DataAnnotations;

namespace FlightApi.Model;

public class Flight
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(10)]
    public string FlightNo { get; set; } = string.Empty;

    public DateOnly FlightDate { get; set; }

    /// <summary>A = Arrival, D = Departure</summary>
    [Required]
    [MaxLength(1)]
    public string ArrDep { get; set; } = string.Empty;

    /// <summary>OPN, DLY, CNX, CLS, XXX</summary>
    [Required]
    [MaxLength(3)]
    public string Status { get; set; } = "OPN";

    public Guid OriginAirportId { get; set; }
    public Airport OriginAirport { get; set; } = null!;

    public Guid DestinationAirportId { get; set; }
    public Airport DestinationAirport { get; set; } = null!;

    [MaxLength(20)]
    public string? AircraftReg { get; set; }

    [Required]
    [MaxLength(10)]
    public string AircraftType { get; set; } = string.Empty;

    /// <summary>PAX | CGO</summary>
    [Required]
    [MaxLength(3)]
    public string FlightType { get; set; } = "PAX";

    [Required]
    [MaxLength(3)]
    public string NatureOfFlight { get; set; } = "---";

    public DateTime? ScheduledTime { get; set; }
    public DateTime? EstimatedTime { get; set; }
    public DateTime? ActualTime { get; set; }

    [MaxLength(10)]
    public string? ParkingStand { get; set; }

    [MaxLength(10)]
    public string? Gate { get; set; }

    public int? BookingPax { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

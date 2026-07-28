using System.ComponentModel.DataAnnotations;

namespace FlightApi.Dto.Request;

public class UpdateFlightRequest
{
    [Required]
    [MaxLength(10)]
    public string FlightNo { get; set; } = string.Empty;

    [Required]
    public DateOnly FlightDate { get; set; }

    [Required]
    [RegularExpression("^[AD]$", ErrorMessage = "ArrDep phải là A hoặc D.")]
    public string ArrDep { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(OPN|DLY|CNX|CLS|XXX)$")]
    public string Status { get; set; } = "OPN";

    [Required]
    public Guid OriginAirportId { get; set; }

    [Required]
    public Guid DestinationAirportId { get; set; }

    [MaxLength(20)]
    public string? AircraftReg { get; set; }

    [Required]
    [MaxLength(10)]
    public string AircraftType { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(PAX|CGO)$")]
    public string FlightType { get; set; } = "PAX";

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
}

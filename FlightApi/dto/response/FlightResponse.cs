namespace FlightApi.Dto.Response;

public class FlightResponse
{
    public Guid Id { get; set; }
    public string FlightNo { get; set; } = string.Empty;
    public DateOnly FlightDate { get; set; }
    public string ArrDep { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public Guid OriginAirportId { get; set; }
    public string OriginIata { get; set; } = string.Empty;
    public Guid DestinationAirportId { get; set; }
    public string DestinationIata { get; set; } = string.Empty;

    public string? AircraftReg { get; set; }
    public string AircraftType { get; set; } = string.Empty;
    public string FlightType { get; set; } = string.Empty;
    public string NatureOfFlight { get; set; } = string.Empty;

    public DateTime? ScheduledTime { get; set; }
    public DateTime? EstimatedTime { get; set; }
    public DateTime? ActualTime { get; set; }

    public string? ParkingStand { get; set; }
    public string? Gate { get; set; }
    public int? BookingPax { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

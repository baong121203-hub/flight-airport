using FlightApi.Dto;
using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Repository;

namespace FlightApi.Service;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;
    private readonly IAirportRepository _airportRepository;

    public FlightService(IFlightRepository flightRepository, IAirportRepository airportRepository)
    {
        _flightRepository = flightRepository;
        _airportRepository = airportRepository;
    }

    public async Task<List<FlightResponse>> GetAllAsync()
    {
        var flights = await _flightRepository.GetAllAsync();
        return FlightMapper.ToResponseList(flights);
    }

    public async Task<FlightResponse?> GetByIdAsync(Guid id)
    {
        var flight = await _flightRepository.GetByIdAsync(id);
        return flight is null ? null : FlightMapper.ToResponse(flight);
    }

    public async Task<FlightResponse> CreateAsync(CreateFlightRequest request)
    {
        await ValidateAirportsAsync(request.OriginAirportId, request.DestinationAirportId);

        var flight = FlightMapper.ToEntity(request);
        var created = await _flightRepository.AddAsync(flight);
        return FlightMapper.ToResponse(created);
    }

    public async Task<FlightResponse?> UpdateAsync(Guid id, UpdateFlightRequest request)
    {
        var flight = await _flightRepository.GetByIdAsync(id);
        if (flight is null)
        {
            return null;
        }

        await ValidateAirportsAsync(request.OriginAirportId, request.DestinationAirportId);

        FlightMapper.ApplyUpdate(flight, request);
        await _flightRepository.UpdateAsync(flight);

        var updated = await _flightRepository.GetByIdAsync(id);
        return FlightMapper.ToResponse(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var flight = await _flightRepository.GetByIdAsync(id);
        if (flight is null)
        {
            return false;
        }

        await _flightRepository.DeleteAsync(flight);
        return true;
    }

    private async Task ValidateAirportsAsync(Guid originId, Guid destinationId)
    {
        if (originId == destinationId)
        {
            throw new InvalidOperationException("Origin và Destination phải khác nhau.");
        }

        if (await _airportRepository.GetByIdAsync(originId) is null)
        {
            throw new InvalidOperationException("OriginAirportId không tồn tại.");
        }

        if (await _airportRepository.GetByIdAsync(destinationId) is null)
        {
            throw new InvalidOperationException("DestinationAirportId không tồn tại.");
        }
    }
}

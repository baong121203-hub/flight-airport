using FlightApi.Dto;
using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Repository;

namespace FlightApi.Service;

public class AirportService : IAirportService
{
    private readonly IAirportRepository _airportRepository;

    public AirportService(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<List<AirportResponse>> GetAllAsync()
    {
        var airports = await _airportRepository.GetAllAsync();
        return AirportMapper.ToResponseList(airports);
    }

    public async Task<AirportResponse?> GetByIdAsync(Guid id)
    {
        var airport = await _airportRepository.GetByIdAsync(id);
        return airport is null ? null : AirportMapper.ToResponse(airport);
    }

    public async Task<AirportResponse> CreateAsync(CreateAirportRequest request)
    {
        if (await _airportRepository.ExistsByIataCodeAsync(request.IataCode))
        {
            throw new InvalidOperationException("IATA code đã tồn tại.");
        }

        var airport = AirportMapper.ToEntity(request);
        var created = await _airportRepository.AddAsync(airport);
        return AirportMapper.ToResponse(created);
    }

    public async Task<AirportResponse?> UpdateAsync(Guid id, UpdateAirportRequest request)
    {
        var airport = await _airportRepository.GetByIdAsync(id);
        if (airport is null)
        {
            return null;
        }

        if (await _airportRepository.ExistsByIataCodeAsync(request.IataCode, id))
        {
            throw new InvalidOperationException("IATA code đã tồn tại.");
        }

        AirportMapper.ApplyUpdate(airport, request);
        await _airportRepository.UpdateAsync(airport);
        return AirportMapper.ToResponse(airport);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var airport = await _airportRepository.GetByIdAsync(id);
        if (airport is null)
        {
            return false;
        }

        await _airportRepository.DeleteAsync(airport);
        return true;
    }
}

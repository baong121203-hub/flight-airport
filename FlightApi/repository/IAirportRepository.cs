using FlightApi.Model;

namespace FlightApi.Repository;

public interface IAirportRepository
{
    Task<List<Airport>> GetAllAsync();
    Task<Airport?> GetByIdAsync(Guid id);
    Task<Airport> AddAsync(Airport airport);
    Task UpdateAsync(Airport airport);
    Task DeleteAsync(Airport airport);
    Task<bool> ExistsByIataCodeAsync(string iataCode, Guid? excludeId = null);
}

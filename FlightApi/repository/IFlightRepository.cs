using FlightApi.Model;

namespace FlightApi.Repository;

public interface IFlightRepository
{
    Task<List<Flight>> GetAllAsync();
    Task<Flight?> GetByIdAsync(Guid id);
    Task<Flight> AddAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    Task DeleteAsync(Flight flight);
}

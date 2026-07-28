using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlightApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class AirportsController : ControllerBase
{
    private readonly IAirportService _airportService;

    public AirportsController(IAirportService airportService)
    {
        _airportService = airportService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AirportResponse>>> GetAirports()
    {
        return Ok(await _airportService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AirportResponse>> GetAirport(Guid id)
    {
        var airport = await _airportService.GetByIdAsync(id);
        if (airport is null)
        {
            return NotFound();
        }

        return Ok(airport);
    }

    [HttpPost]
    public async Task<ActionResult<AirportResponse>> CreateAirport(CreateAirportRequest request)
    {
        try
        {
            var created = await _airportService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAirport), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AirportResponse>> UpdateAirport(Guid id, UpdateAirportRequest request)
    {
        try
        {
            var updated = await _airportService.UpdateAsync(id, request);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAirport(Guid id)
    {
        var deleted = await _airportService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

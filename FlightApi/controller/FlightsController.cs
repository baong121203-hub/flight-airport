using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlightApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightsController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlightResponse>>> GetFlights()
    {
        return Ok(await _flightService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlightResponse>> GetFlight(Guid id)
    {
        var flight = await _flightService.GetByIdAsync(id);
        if (flight is null)
        {
            return NotFound();
        }

        return Ok(flight);
    }

    [HttpPost]
    public async Task<ActionResult<FlightResponse>> CreateFlight(CreateFlightRequest request)
    {
        try
        {
            var created = await _flightService.CreateAsync(request);
            return CreatedAtAction(nameof(GetFlight), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FlightResponse>> UpdateFlight(Guid id, UpdateFlightRequest request)
    {
        try
        {
            var updated = await _flightService.UpdateAsync(id, request);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFlight(Guid id)
    {
        var deleted = await _flightService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

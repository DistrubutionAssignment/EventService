using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Dtos;
using Presentation.Factories;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] //global auth för alla actions
public class EventController : ControllerBase 
{

    private readonly DataContext _context;
    public EventController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous] //tillåter alla användare att se events även om dom inte är inloggade
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
    {
        var events = await _context.Events
            .OrderBy(e => e.Date)
            .ToListAsync();

        return Ok(events.Select(e => EventFactory.ToDto(e)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(string id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null)
        {

            return NotFound();
        }
        return Ok(EventFactory.ToDto(e));
    }

    [HttpPost] 
    [Authorize(Roles = "Admin")] // Endast administratörer kan skapa events Ej implementerat i frontend, men fungerar via swagger.
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
    {
        var e = EventFactory.CreateFromDto(dto);
        _context.Events.Add(e);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = e.Id }, EventFactory.ToDto(e));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Endast administratörer kan uppdatera events Ej implementerat i frontend, men fungerar via swagger.
    public async Task<IActionResult> UpdateEvent(string id, UpdateEventDto dto)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null)
        {
            return NotFound();
        }

        EventFactory.UpdateFromDto(e, dto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Endast administratörer kan ta bort events Ej implementerat i frontend, men fungerar via swagger.
    public async Task<IActionResult> DeleteEvent(string id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null)
        {
            return NotFound();
        }
        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

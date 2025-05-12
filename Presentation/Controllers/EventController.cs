using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Dtos;
using Presentation.Factories;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{

    private readonly DataContext _context;
    public EventController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
    {
        var events = await _context.Events
            .OrderBy(e => e.Date)
            .ToListAsync();

        return Ok(events.Select(EventFactory.ToDto));
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
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
    {
        var e = EventFactory.CreateFromDto(dto);
        _context.Events.Add(e);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = e.Id }, EventFactory.ToDto(e));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, UpdateEventDto dto)
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

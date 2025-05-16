using Presentation.Models;
using Presentation.Dtos;

namespace Presentation.Factories;

public static class EventFactory
{
    public static EventModel CreateFromDto(CreateEventDto dto)
    {
        return new EventModel
        {
            Name = dto.Name,
            Description = dto.Description,
            Location = dto.Location,
            Date = dto.Date,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl
        };
    }

    public static void UpdateFromDto(EventModel existing, UpdateEventDto dto)
    {
        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Location = dto.Location;
        existing.Date = dto.Date;
        existing.Price = dto.Price;
        existing.ImageUrl = dto.ImageUrl;
    }

    public static EventDto ToDto(EventModel e)
    {
        return new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Location = e.Location,
            Date = e.Date,
            Price = e.Price,
            ImageUrl = e.ImageUrl
        };
    }
}
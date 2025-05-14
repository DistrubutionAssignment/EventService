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
            Catagory = dto.Catagory,
            Location = dto.Location,
            Date = dto.Date,
            MaxTickets = dto.MaxTickets,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl
        };
    }

    public static void UpdateFromDto(EventModel existing, UpdateEventDto dto)
    {
        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Catagory = dto.Catagory;
        existing.Location = dto.Location;
        existing.Date = dto.Date;
        existing.MaxTickets = dto.MaxTickets;
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
            Catagory = e.Catagory,
            Location = e.Location,
            Date = e.Date,
            MaxTickets = e.MaxTickets,
            Price = e.Price,
            ImageUrl = e.ImageUrl
        };
    }
}
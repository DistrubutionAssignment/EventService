namespace Presentation.Models;

public class EventModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Catagory { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime Date { get; set; }
    public int MaxTickets { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}

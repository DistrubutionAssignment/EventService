namespace Presentation.Dtos;

public class EventDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
}

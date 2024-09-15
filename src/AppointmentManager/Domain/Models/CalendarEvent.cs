namespace Domain.Models;

public class CalendarEvent
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
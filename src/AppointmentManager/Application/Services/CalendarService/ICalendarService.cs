using Domain.Models;

namespace Application.Services.CalendarService;

public interface ICalendarService
{
    string CalendarId { get; }
    string CalendarToken { get; }
    Task<CalendarEvent> AddEvent(CalendarEvent calendarEvent, CancellationToken cancellationToken);
    Task DeleteEvent(string eventId, CancellationToken cancellationToken);
    Task<CalendarEvent> UpdateEvent(CalendarEvent calendarEvent, CancellationToken cancellationToken);
    Task<CalendarEvent> UpdateEventColor(string eventId, string color, CancellationToken cancellationToken);
    Task<CalendarEvent> GetEvent(string eventId);
    Task<IEnumerable<CalendarEvent>> GetUpdatedEvents();
}
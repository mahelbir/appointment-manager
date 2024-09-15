using Domain.Models;

namespace Application.Services.CalendarService;

public interface ICalendarService
{
    string CalendarId { get; }
    string CalendarToken { get; }
    Task<CalendarEvent> AddEvent(CalendarEvent calendarEvent);
    Task DeleteEvent(string eventId);
    Task<CalendarEvent> GetEvent(string eventId);
    Task<CalendarEvent> UpdateEvent(CalendarEvent calendarEvent);
    Task<CalendarEvent> UpdateEventColor(string eventId, string color);
}
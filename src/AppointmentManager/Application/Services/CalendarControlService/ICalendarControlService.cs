using Domain.Entities;
using Domain.Models;

namespace Application.Services.CalendarControlService;

public interface ICalendarControlService
{
    Task ValidateCalendarToken(string token);
    Task<CalendarEvent> AddCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task DeleteCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task<CalendarEvent> UpdateCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task<CalendarEvent> UpdateCalendarEventColor(Appointment appointment, CancellationToken cancellationToken);
    Task<IEnumerable<CalendarEvent>> UpdateCalendarEventsClient(IEnumerable<Appointment> appointments,
        Client client, CancellationToken cancellationToken);
    Task<IEnumerable<CalendarEvent>> GetUpdatedEvents();
}
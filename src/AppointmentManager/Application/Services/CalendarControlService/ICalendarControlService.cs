using Domain.Entities;
using Domain.Models;

namespace Application.Services.CalendarControlService;

public interface ICalendarControlService
{
    Task ValidateCalendarToken(string token);
    Task<Appointment> AddCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task<Appointment> UpdateCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task DeleteCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task UpdateCalendarEventColor(Appointment appointment, CancellationToken cancellationToken);
    Task<IEnumerable<CalendarEvent>> GetUpdatedEvents();

}
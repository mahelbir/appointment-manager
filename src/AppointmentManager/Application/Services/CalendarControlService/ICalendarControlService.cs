using Domain.Entities;

namespace Application.Services.CalendarControlService;

public interface ICalendarControlService
{
    Task<Appointment> AddCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task<Appointment> UpdateCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task DeleteCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
    Task UpdateCalendarEventColor(Appointment appointment, CancellationToken cancellationToken);
}
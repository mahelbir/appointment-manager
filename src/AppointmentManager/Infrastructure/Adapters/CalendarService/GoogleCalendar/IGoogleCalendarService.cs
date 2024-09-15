using Application.Services.CalendarService;
using Domain.Models;

namespace Infrastructure.Adapters.CalendarService.GoogleCalendar;

public interface IGoogleCalendarService: ICalendarService
{
    Task<IEnumerable<CalendarEvent>> GetUpdatedEvents();
    Task<GoogleCalendarChannel> StartWatching(string webhookUrl, CancellationToken cancellationToken);
    Task StopWatching(GoogleCalendarChannel channel, CancellationToken cancellationToken);
}
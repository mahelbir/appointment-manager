using Application.Services.CalendarService;

namespace Infrastructure.Adapters.CalendarService.GoogleCalendar.Models;

public interface IGoogleCalendarService: ICalendarService
{
    Task<GoogleCalendarChannel> StartWatching(string webhookUrl, CancellationToken cancellationToken);
    Task StopWatching(GoogleCalendarChannel channel, CancellationToken cancellationToken);
}
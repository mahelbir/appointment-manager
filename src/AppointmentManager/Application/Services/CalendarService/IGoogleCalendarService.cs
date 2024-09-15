using Domain.Models;

namespace Application.Services.CalendarService;

public interface IGoogleCalendarService: ICalendarService
{
    Task<IEnumerable<CalendarEvent>> GetUpdatedEvents();
    Task<GoogleCalendarChannel> StartWatching(string webhookUrl);
    Task StopWatching(GoogleCalendarChannel channel);
}
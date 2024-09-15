namespace Application.Services.CalendarSyncService;

public interface ICalendarSyncService
{
    Task StartAsync(CancellationToken stoppingToken);
    Task StopAsync(CancellationToken cancellationToken);
}
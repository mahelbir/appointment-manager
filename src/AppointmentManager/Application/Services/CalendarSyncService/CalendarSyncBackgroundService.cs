using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services.CalendarSyncService;

public class CalendarSyncBackgroundService : BackgroundService
{
    private readonly ILogger<CalendarSyncBackgroundService> _logger;
    ICalendarSyncService _calendarSyncService;

    public CalendarSyncBackgroundService(ILogger<CalendarSyncBackgroundService> logger,
        ICalendarSyncService calendarSyncService)
    {
        _logger = logger;
        _calendarSyncService = calendarSyncService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Calendar Sync Background Service started");
        await _calendarSyncService.StartAsync(stoppingToken);
        _logger.LogInformation("Calendar Sync Background Service executed");
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _calendarSyncService.StopAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
        _logger.LogInformation("Calendar Sync Background Service stopped");
    }
    
}
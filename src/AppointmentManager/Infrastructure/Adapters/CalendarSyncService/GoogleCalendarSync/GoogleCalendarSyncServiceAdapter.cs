using Infrastructure.Adapters.CalendarService.GoogleCalendar.Models;
using Application.Services.CalendarSyncService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Adapters.CalendarSyncService.GoogleCalendarSync;

class GoogleCalendarSyncServiceAdapter : ICalendarSyncService
{
    private readonly ILogger<GoogleCalendarSyncServiceAdapter> _logger;
    private readonly IGoogleCalendarService _googleCalendarService;
    private GoogleCalendarChannel _channel;

    public GoogleCalendarSyncServiceAdapter(ILogger<GoogleCalendarSyncServiceAdapter> logger, IGoogleCalendarService googleCalendarService, IConfiguration configuration)
    {
        _logger = logger;
        _googleCalendarService = googleCalendarService;
        _channel = new GoogleCalendarChannel
        {
            WebHookUrl = configuration["GoogleCalendar:WebhookUrl"] ?? ""
        };
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await StopAsync(stoppingToken);

                if (string.IsNullOrEmpty(_channel.WebHookUrl))
                {
                    throw new ArgumentException("Webhook URL error");
                }

                _channel = await _googleCalendarService.StartWatching(_channel.WebHookUrl, stoppingToken);
                _logger.LogInformation($"Started watching Google Calendar. Channel ID: {_channel.Id}");

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while managing Google Calendar watch channels");
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_channel.Id))
        {
            await _googleCalendarService.StopWatching(_channel, cancellationToken);
            _logger.LogInformation($"Stopped watching calendar. Channel ID: {_channel.Id} on service stop.");
        }
    }
    
}
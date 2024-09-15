using AutoMapper;
using Domain.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using GoogleCalendarService = Google.Apis.Calendar.v3.CalendarService;

namespace Infrastructure.Adapters.CalendarService.GoogleCalendar;

public class GoogleCalendarServiceAdapter : IGoogleCalendarService
{
    private readonly IMapper _mapper;
    private readonly GoogleCalendarService _calendarService;

    public string CalendarId { get; }
    public string CalendarToken { get; }

    public GoogleCalendarServiceAdapter(IMapper mapper, IConfiguration configuration)
    {
        var serviceAccountKeyFilePath = configuration["GoogleCalendar:ServiceAccountFilePath"] ?? "";
        CalendarId = configuration["GoogleCalendar:CalendarId"] ?? "";
        CalendarToken = GetCalendarToken();
        _calendarService = Init(serviceAccountKeyFilePath);
        _mapper = mapper;
    }

    private GoogleCalendarService Init(string serviceAccountFilePath)
    {
        if (string.IsNullOrEmpty(CalendarId))
        {
            throw new ArgumentException("Google Calendar Id Error");
        }

        if (string.IsNullOrEmpty(serviceAccountFilePath))
        {
            throw new ArgumentException("Google Calendar Service Account File Path Error");
        }

        GoogleCredential credential;
        using (var stream = new FileStream(serviceAccountFilePath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(GoogleCalendarService.Scope.Calendar);
        }
        
        //Console.WriteLine(CalendarId + " " + CalendarToken);

        return new GoogleCalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            }
        );
    }

    private string GetCalendarToken()
    {
        return BitConverter.ToString(System.Text.Encoding.Default.GetBytes(CalendarId));
    }

    private static Event EventBody(CalendarEvent calendarEvent)
    {
        return new Event()
        {
            Summary = calendarEvent.Title,
            Description = calendarEvent.Description,
            ColorId = calendarEvent.Color,
            Start = new EventDateTime()
            {
                DateTimeDateTimeOffset = calendarEvent.StartDate.ToUniversalTime(),
                TimeZone = "UTC"
            },
            End = new EventDateTime()
            {
                DateTimeDateTimeOffset = calendarEvent.EndDate.ToUniversalTime(),
                TimeZone = "UTC"
            }
        };
    }

    public async Task<CalendarEvent> AddEvent(CalendarEvent calendarEvent, CancellationToken cancellationToken)
    {
        var eventBody = EventBody(calendarEvent);
        var result = await _calendarService.Events.Insert(eventBody, CalendarId)
            .ExecuteAsync(cancellationToken: cancellationToken);
        return _mapper.Map<CalendarEvent>(result);
    }

    public async Task DeleteEvent(string eventId, CancellationToken cancellationToken)
    {
        await _calendarService.Events.Delete(CalendarId, eventId)
            .ExecuteAsync(cancellationToken: cancellationToken);
    }

    public async Task<CalendarEvent> UpdateEvent(CalendarEvent calendarEvent, CancellationToken cancellationToken)
    {
        var eventBody = EventBody(calendarEvent);
        var result = await _calendarService.Events.Patch(eventBody, CalendarId, calendarEvent.Id)
            .ExecuteAsync(cancellationToken: cancellationToken);
        return _mapper.Map<CalendarEvent>(result);
    }

    public async Task<CalendarEvent> UpdateEventColor(string eventId, string color, CancellationToken cancellationToken)
    {
        var eventBody = new Event
        {
            ColorId = color,
        };
        var result = await _calendarService.Events.Patch(eventBody, CalendarId, eventId)
            .ExecuteAsync(cancellationToken: cancellationToken);
        return _mapper.Map<CalendarEvent>(result);
    }

    private async Task<Event> GetEventData(string eventId)
    {
        return await _calendarService.Events.Get(CalendarId, eventId).ExecuteAsync();
    }

    public async Task<CalendarEvent> GetEvent(string eventId)
    {
        var result = await GetEventData(eventId);
        return _mapper.Map<CalendarEvent>(result);
    }

    public async Task<IEnumerable<CalendarEvent>> GetUpdatedEvents()
    {
        var q = _calendarService.Events.List(CalendarId);
        q.TimeZone = TimeZoneInfo.Local.ToString();
        q.UpdatedMinDateTimeOffset = DateTimeOffset.Now.AddDays(-14);
        q.MaxResults = 2500;
        var result = await q.ExecuteAsync();
        return result.Items.Reverse().Select(e => _mapper.Map<CalendarEvent>(e));
    }

    public async Task<GoogleCalendarChannel> StartWatching(string webhookUrl, CancellationToken cancellationToken)
    {
        var channel = new Channel()
        {
            Id = Guid.NewGuid().ToString(),
            Type = "web_hook",
            Address = webhookUrl,
            Token = CalendarToken
        };
        channel = await _calendarService.Events.Watch(channel, CalendarId)
            .ExecuteAsync(cancellationToken: cancellationToken);
        var result = _mapper.Map<GoogleCalendarChannel>(channel);
        result.WebHookUrl = webhookUrl;
        return result;
    }

    public async Task StopWatching(GoogleCalendarChannel channel, CancellationToken cancellationToken)
    {
        var googleCalendarChannel = _mapper.Map<Channel>(channel);
        await _calendarService.Channels.Stop(googleCalendarChannel)
            .ExecuteAsync(cancellationToken: cancellationToken);
    }
    
}
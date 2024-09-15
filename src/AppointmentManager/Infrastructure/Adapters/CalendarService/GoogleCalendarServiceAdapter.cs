using System.Text;
using System.Security.Cryptography;
using Application.Services.CalendarService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Domain.Models;
using GoogleCalendarService = Google.Apis.Calendar.v3.CalendarService;

namespace Infrastructure.Adapters.CalendarService;

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
        CalendarToken = GenCalendarToken();
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

        return new GoogleCalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            }
        );
    }

    private string GenCalendarToken()
    {
        var input = CalendarId + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
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
                DateTimeDateTimeOffset = calendarEvent.StartDate.ToUniversalTime(),
                TimeZone = "UTC"
            }
        };
    }

    public async Task<CalendarEvent> AddEvent(CalendarEvent calendarEvent)
    {
        var eventBody = EventBody(calendarEvent);
        var result = await _calendarService.Events.Insert(eventBody, CalendarId).ExecuteAsync();
        return _mapper.Map<CalendarEvent>(result);
    }

    public async Task DeleteEvent(string eventId)
    {
        await _calendarService.Events.Delete(CalendarId, eventId).ExecuteAsync();
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

    public async Task<CalendarEvent> UpdateEvent(CalendarEvent dto)
    {
        var eventBody = EventBody(dto);
        var result = await _calendarService.Events.Patch(eventBody, CalendarId, dto.Id).ExecuteAsync();
        return _mapper.Map<CalendarEvent>(result);
    }

    public async Task<CalendarEvent> UpdateEventColor(string eventId, string colorId)
    {
        var eventBody = new Event
        {
            ColorId = colorId,
        };
        var result = await _calendarService.Events.Patch(eventBody, CalendarId, eventId).ExecuteAsync();
        return _mapper.Map<CalendarEvent>(result);
    }

    public async Task<GoogleCalendarChannel> StartWatching(string webhookUrl)
    {
        var channel = new Channel()
        {
            Id = Guid.NewGuid().ToString(),
            Type = "web_hook",
            Address = webhookUrl,
            Token = CalendarToken
        };
        var result = await _calendarService.Events.Watch(channel, CalendarId).ExecuteAsync();
        return _mapper.Map<GoogleCalendarChannel>(result);
    }

    public async Task StopWatching(GoogleCalendarChannel channel)
    {
        var googleCalendarChannel = _mapper.Map<Channel>(channel);
        await _calendarService.Channels.Stop(googleCalendarChannel).ExecuteAsync();
    }
}
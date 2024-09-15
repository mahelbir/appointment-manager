using System.Reflection;
using Application.Services.CalendarService;
using Application.Services.CalendarSyncService;
using Infrastructure.Adapters.CalendarService.GoogleCalendar;
using Infrastructure.Adapters.CalendarSyncService.GoogleCalendarSync;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ICalendarService, GoogleCalendarServiceAdapter>();
        services.AddSingleton<IGoogleCalendarService, GoogleCalendarServiceAdapter>();
        services.AddSingleton<ICalendarSyncService, GoogleCalendarSyncServiceAdapter>();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
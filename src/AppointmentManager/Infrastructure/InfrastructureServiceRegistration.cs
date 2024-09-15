using System.Reflection;
using Application.Services.CalendarService;
using Infrastructure.Adapters.CalendarService;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ICalendarService, GoogleCalendarServiceAdapter>();
        services.AddSingleton<IGoogleCalendarService, GoogleCalendarServiceAdapter>();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
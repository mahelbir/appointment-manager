using Application.Extensions;
using Domain.Enums;

namespace Application.Services.AppointmentService;

public class AppointmentManager : IAppointmentService
{
    public List<AppointmentStatus> GetAppointmentStatusList()
    {
        return Enum
            .GetValues(typeof(AppointmentStatus))
            .Cast<AppointmentStatus>()
            .ToList();
    }
    
    public List<AppointmentStatus> GetVisibleAppointmentStatusList()
    {
        var statuses = GetAppointmentStatusList();
        return statuses
            .Where(s => s.GetProps().IsVisible)
            .ToList();
    }
    
}
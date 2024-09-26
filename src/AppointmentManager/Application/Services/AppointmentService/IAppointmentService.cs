using Domain.Entities;
using Domain.Enums;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
    List<AppointmentStatus> GetAppointmentStatusList();
    List<AppointmentStatus> GetVisibleAppointmentStatusList();
    Client ParseClientFromDescription(string description);
}
using Domain.Enums;
using Domain.Models;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
    List<AppointmentStatus> GetAppointmentStatusList();
    List<AppointmentStatus> GetVisibleAppointmentStatusList();
}
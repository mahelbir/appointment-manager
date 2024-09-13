using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
     Task<IEnumerable<Appointment>> GetListByDateRange(DateOnly startDate, DateOnly endDate);
     AppointmentStatus[] GetVisibleAppointmentStatuses();
     AppointmentStatusProps GetAppointmentStatus(AppointmentStatus status);
     IDictionary<AppointmentStatus, AppointmentStatusProps> GetAppointmentStatuses();
}
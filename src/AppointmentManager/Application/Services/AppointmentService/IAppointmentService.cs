using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
     AppointmentStatus[] GetVisibleAppointmentStatuses();
     AppointmentStatusProps GetAppointmentStatus(AppointmentStatus status);
     IDictionary<AppointmentStatus, AppointmentStatusProps> GetAppointmentStatuses();
     Task<IEnumerable<Appointment>> GetListByDateRange(DateOnly startDate, DateOnly endDate);
     Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateOnly startDate, DateOnly endDate);
}
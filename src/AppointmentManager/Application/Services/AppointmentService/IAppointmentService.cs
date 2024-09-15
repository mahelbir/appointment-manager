using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
     Task<IEnumerable<Appointment>> GetListByDateRange(DateOnly startDate, DateOnly endDate);
     Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateOnly startDate, DateOnly endDate);
     Task<Appointment> CreateCalendarEvent(Appointment appointment);
     AppointmentStatus[] GetVisibleAppointmentStatuses();
     AppointmentStatusProps GetAppointmentStatus(AppointmentStatus status);
     IDictionary<AppointmentStatus, AppointmentStatusProps> GetAppointmentStatuses();
}
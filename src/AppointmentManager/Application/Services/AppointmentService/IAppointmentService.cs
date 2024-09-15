using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
     Task<IEnumerable<Appointment>> GetListByDateRange(DateOnly startDate, DateOnly endDate);
     Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateOnly startDate, DateOnly endDate);
     Task<Appointment> AddCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
     Task<Appointment> UpdateCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
     Task DeleteCalendarEvent(Appointment appointment, CancellationToken cancellationToken);
     Task UpdateCalendarEventColor(Appointment appointment, CancellationToken cancellationToken);
     AppointmentStatus[] GetVisibleAppointmentStatuses();
     AppointmentStatusProps GetAppointmentStatus(AppointmentStatus status);
     IDictionary<AppointmentStatus, AppointmentStatusProps> GetAppointmentStatuses();
}
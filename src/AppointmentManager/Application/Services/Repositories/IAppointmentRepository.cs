using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IAppointmentRepository : IAsyncRepository<Appointment, int>
{
    Task<IEnumerable<Appointment>> GetListByDateRange(DateTime startDate, DateTime endDate, List<AppointmentStatus> statusList);
    Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateTime startDate, DateTime endDate);
    Task<Appointment?> GetByCalendarEventId(string calendarEventId);
    Task<bool> IsOverlap(DateTime startDate, DateTime endDate);
    Task<bool> IsOverlap(DateTime startDate, DateTime endDate, int id);
}
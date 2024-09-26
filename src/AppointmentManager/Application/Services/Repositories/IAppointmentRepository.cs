using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IAppointmentRepository : IAsyncRepository<Appointment, int>
{
    IQueryable<Appointment> InStatus(IEnumerable<AppointmentStatus> statuses);

    Task<IEnumerable<Appointment>> GetListByDateRange(IQueryable<Appointment> query, DateTime startDate,
        DateTime endDate);

    Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateTime startDate, DateTime endDate);
    Task<Appointment?> GetByCalendarEventId(string calendarEventId);
    Task<bool> IsOverlap(IQueryable<Appointment> query, DateTime startDate, DateTime endDate);
    Task<bool> IsOverlap(IQueryable<Appointment> query, DateTime startDate, DateTime endDate, int id);
}
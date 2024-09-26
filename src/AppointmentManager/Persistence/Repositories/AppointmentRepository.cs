using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class AppointmentRepository : EfRepositoryBase<Appointment, int, BaseDbContext>, IAppointmentRepository
{
    private IAppointmentRepository _appointmentRepositoryImplementation;

    public AppointmentRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Appointment>> GetListByDateRange(DateTime startDate, DateTime endDate,
        List<AppointmentStatus> statusList)
    {
        return await Query()
            .Where(a => statusList.Contains(a.Status))
            .Where(a =>
                a.StartDate >= startDate &&
                a.EndDate <= endDate
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateTime startDate, DateTime endDate)
    {
        return await Query()
            .Include(a => a.Client)
            .Where(a =>
                a.StartDate >= startDate &&
                a.EndDate <= endDate
            )
            .ToListAsync();
    }

    public async Task<Appointment?> GetByCalendarEventId(string calendarEventId)
    {
        return await GetAsync(
            predicate: a => a.CalendarEventId == calendarEventId,
            include: a => a.Include(a => a.Client)
        );
    }

    public async Task<bool> IsOverlap(DateTime startDate, DateTime endDate)
    {
        return await IsOverlap(Query(), startDate, endDate);
    }

    public async Task<bool> IsOverlap(DateTime startDate, DateTime endDate, int id)
    {
        var query = Query()
            .Where(a => a.Id != id);
        return await IsOverlap(query, startDate, endDate);
    }

    private async Task<bool> IsOverlap(IQueryable<Appointment> query, DateTime startDate, DateTime endDate)
    {
        return await query
            .Where(a =>
                    (startDate >= a.StartDate && startDate < a.EndDate) || // başlangıç mevcut randevunun içinde 
                    (endDate > a.StartDate && endDate <= a.EndDate) || // bitiş mevcut randevunun içinde
                    (startDate <= a.StartDate && endDate >= a.EndDate) // yeni randevu mevcut randevuyu kapsıyor
            )
            .AnyAsync();
    }
}
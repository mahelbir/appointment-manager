using Domain.Models;

namespace Application.Features.Appointments.Queries.GetDetailedCalendar;

public class GetDetailedCalendarAppointmentListItemDto: CalendarAppointment
{
    public int ClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
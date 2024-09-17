using Application.Features.Appointments.Queries.GetCalendar;
using Application.Features.Clients.Queries.GetById;

namespace Application.Features.Appointments.Queries.GetDetailedCalendar;

public class GetDetailedCalendarAppointmentListItemDto: GetCalendarAppointmentListItemDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public GetByIdClientResponse Client { get; set; }
    
}
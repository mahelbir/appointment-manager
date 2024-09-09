using Domain.Enums;

namespace Application.Features.Appointments.Queries.GetList;

public class GetListAppointmentListItemDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
}
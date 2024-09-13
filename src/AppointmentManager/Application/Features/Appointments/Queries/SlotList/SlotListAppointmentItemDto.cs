using Domain.Enums;
using Domain.Models;

namespace Application.Features.Appointments.Queries.SlotList;

public class SlotListAppointmentItemDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentStatusProps Props { get; set; }
}
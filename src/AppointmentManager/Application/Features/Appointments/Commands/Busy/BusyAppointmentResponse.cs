using Domain.Enums;

namespace Application.Features.Appointments.Commands.Busy;

public class BusyAppointmentResponse
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
}
using Domain.Enums;

namespace Application.Features.Appointments.Commands.Update;

public class UpdatedAppointmentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime UpdatedDate { get; set; }
}
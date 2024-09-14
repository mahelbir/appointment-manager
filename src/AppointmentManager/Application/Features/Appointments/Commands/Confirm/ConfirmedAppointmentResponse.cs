using Domain.Enums;

namespace Application.Features.Appointments.Commands.Confirm;

public class ConfirmedAppointmentResponse
{
    public int Id { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime UpdatedDate { get; set; }
}
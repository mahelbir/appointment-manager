using Domain.Enums;

namespace Application.Features.Appointments.Commands.Cancel;

public class CanceledAppointmentResponse
{
    public int Id { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime UpdatedDate { get; set; }
}
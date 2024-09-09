namespace Application.Features.Appointments.Commands.Delete;

public class DeletedAppointmentResponse
{
    public int Id { get; set; }
    public DateTime DeletedDate { get; set; }
}
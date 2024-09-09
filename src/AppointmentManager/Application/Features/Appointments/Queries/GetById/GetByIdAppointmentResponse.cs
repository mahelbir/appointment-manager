using Domain.Enums;

namespace Application.Features.Appointments.Queries.GetById;

public class GetByIdAppointmentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
}
using Domain.Enums;

namespace Application.Features.Appointments.Queries.GetById;

public class GetByIdAppointmentResponse
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public AppointmentStatus Status { get; set; }
}
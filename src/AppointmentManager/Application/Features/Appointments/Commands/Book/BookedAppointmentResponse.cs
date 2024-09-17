using Application.Features.Clients.Queries.GetById;
using Domain.Enums;

namespace Application.Features.Appointments.Commands.Book;

public class BookedAppointmentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public GetByIdClientResponse Client { get; set; }
}
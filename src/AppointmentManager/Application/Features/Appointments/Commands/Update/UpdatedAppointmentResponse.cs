using Domain.Enums;

namespace Application.Features.Appointments.Commands.Update;

public class UpdatedAppointmentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime UpdatedDate { get; set; }
    public UpdatedAppointmentResponseClient Client { get; set; }

    public class UpdatedAppointmentResponseClient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}